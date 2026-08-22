using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Common.Cloudinary;
using ECommerce.Application.Products.Errors;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductHandler(
    IUnitOfWork unitOfWork,
    IImageService imageService)
    : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IImageService _imageService = imageService;

    public async Task<Result> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.Repository<Product>();
        var brandRepository = _unitOfWork.Repository<ProductBrand>();
        var typeRepository = _unitOfWork.Repository<ProductType>();

        // ---------- Load Product ----------

        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.Id),
            cancellationToken);

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        // ---------- Business Validation ----------

        if (request.BrandId.HasValue)
        {
            var brandExists = await brandRepository.AnyAsync(
                new BrandByIdSpecification(request.BrandId.Value),
                cancellationToken);

            if (!brandExists)
                return Result.Failure(BrandErrors.NotFound);
        }

        if (request.TypeId.HasValue)
        {
            var typeExists = await typeRepository.AnyAsync(
                new ProductTypeByIdSpecification(request.TypeId.Value),
                cancellationToken);

            if (!typeExists)
                return Result.Failure(TypeErrors.NotFound);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var exists = await productRepository.AnyAsync(
                new ProductNameSpecification(
                    request.Name,
                    request.Id),
                cancellationToken);

            if (exists)
                return Result.Failure(ProductErrors.ProductAlreadyExists);
        }

        // ---------- Update Entity ----------

        if (!string.IsNullOrWhiteSpace(request.Name))
            product.SetName(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Description))
            product.SetDescription(request.Description);

        // ---------- Update Picture ----------

        string? oldPicturePublicId = null;

        if (request.Picture is not null)
        {
            // Keep the old ID so we can delete the old image
            // after the database update succeeds.
            oldPicturePublicId = product.PicturePublicId;

            var uploadResult = await _imageService.UploadAsync(
                request.Picture.Content,
                request.Picture.FileName,
                cancellationToken);

            product.SetPictureUrl(
                uploadResult.Url,
                uploadResult.PublicId);
        }

        if (request.Price.HasValue)
            product.ChangePrice(request.Price.Value);

        if (request.BrandId.HasValue)
            product.ChangeBrand(request.BrandId.Value);

        if (request.TypeId.HasValue)
            product.ChangeProductType(request.TypeId.Value);

        // ---------- Persist ----------

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        // ---------- Delete Old Picture ----------

        if (!string.IsNullOrWhiteSpace(oldPicturePublicId))
        {
            await _imageService.DeleteAsync(
                oldPicturePublicId,
                cancellationToken);
        }

        // ---------- Result ----------

        return Result.Success();
    }
}