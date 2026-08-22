using ECommerce.Application.Common.Cloudinary;

public interface IImageService
{
    Task<ImageUploadResult> UploadAsync(
        Stream image,
        string fileName,
        CancellationToken ct = default);

    Task DeleteAsync(
        string publicId,
        CancellationToken ct = default);
}