using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Common.Cloudinary;
using ECommerce.Application.Products.Errors;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;

using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed class CreateProductHandler(
    IUnitOfWork unitOfWork,
    IImageService imageService)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IImageService _imageService = imageService;

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.Repository<Product>();
        var brandRepository = _unitOfWork.Repository<ProductBrand>();
        var typeRepository = _unitOfWork.Repository<ProductType>();

        // ---------- Business Validation ----------

        var brandExists = await brandRepository.AnyAsync(
            new BrandByIdSpecification(request.BrandId),
            cancellationToken);

        if (!brandExists)
            return Result<Guid>.Failure(BrandErrors.AlreadyExists);

        var typeExists = await typeRepository.AnyAsync(
            new ProductTypeByIdSpecification(request.TypeId),
            cancellationToken);

        if (!typeExists)
            return Result<Guid>.Failure(TypeErrors.AlreadyExists);

        var productExists = await productRepository.AnyAsync(
            new ProductNameSpecification(request.ProductName),
            cancellationToken);

        if (productExists)
            return Result<Guid>.Failure(ProductErrors.ProductAlreadyExists);

        // ---------- Upload Image ----------

        if (request.Picture is null)
            throw new ArgumentException("Product picture is required.");

        var uploadResult = await _imageService.UploadAsync(
            request.Picture.Content,
            request.Picture.FileName,
            cancellationToken);

        // ---------- Create Entity ----------

        var product = Product.Create(
            request.ProductName,
            request.ProductDescription,
            uploadResult.Url,
            uploadResult.PublicId,
            request.Price,
            request.BrandId,
            request.TypeId);

        // ---------- Persist ----------

        productRepository.Create(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ---------- Result ----------

        return Result<Guid>.Success(product.Id);
    }
}