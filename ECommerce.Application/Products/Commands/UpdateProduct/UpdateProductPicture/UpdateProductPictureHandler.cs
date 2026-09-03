using ECommerce.Application.Common.Cloudinary;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Products.Commands.UpdateProductPicture;

public sealed class UpdateProductPictureHandler(
    IUnitOfWork unitOfWork,
    IImageService imageService)
    : IRequestHandler<UpdateProductPictureCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IImageService _imageService = imageService;

    public async Task<Result<bool>> Handle(
        UpdateProductPictureCommand request,
        CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.Repository<Product>();

        // ---------- Load Product ----------

        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.ProductId),
            cancellationToken);

        if (product is null)
            return Result<bool>.Failure(ProductErrors.NotFound);

        // ---------- Keep Old Picture ----------

        var oldPicturePublicId = product.PicturePublicId;

        // ---------- Upload New Picture ----------

        var uploadResult = await _imageService.UploadAsync(
            request.Picture.Content,
            request.Picture.FileName,
            cancellationToken);

        try
        {
            // ---------- Update Product ----------

            product.SetPictureUrl(
                uploadResult.Url,
                uploadResult.PublicId);

            // ---------- Persist ----------

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            // Database update failed.
            // Remove the newly uploaded Cloudinary image
            // because the database is still pointing to the old one.

            await _imageService.DeleteAsync(
                uploadResult.PublicId,
                cancellationToken);

            throw;
        }

        // ---------- Delete Old Picture ----------

        if (!string.IsNullOrWhiteSpace(oldPicturePublicId))
        {
            await _imageService.DeleteAsync(
                oldPicturePublicId,
                cancellationToken);
        }

        return Result<bool>.Success(true);
    }
}