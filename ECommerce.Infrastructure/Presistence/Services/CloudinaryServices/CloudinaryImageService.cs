using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.Application.Common.Cloudinary;
using ECommerce.Infrastructure.Services.Cloudinary;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.CloudinaryServices;

public sealed class CloudinaryImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageService(
        IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;

        var account = new Account(
            settings.CloudName,
            settings.ApiKey,
            settings.ApiSecret);

        _cloudinary = new Cloudinary(account)
        {
            Api = { Secure = true }
        };
    }

    public async Task<Application.Common.Cloudinary.ImageUploadResult> UploadAsync(
        Stream image,
        string fileName,
        CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, image),
            Folder = "products"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
        {
            throw new InvalidOperationException(
                $"Cloudinary upload failed: {result.Error.Message}");
        }

        return new Application.Common.Cloudinary.ImageUploadResult(
            result.SecureUrl.ToString(),
            result.PublicId);
    }

    public async Task DeleteAsync(
        string publicId,
        CancellationToken ct = default)
    {
        var deleteParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image,
            Invalidate = true
        };

        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error is not null)
        {
            throw new InvalidOperationException(
                $"Cloudinary deletion failed: {result.Error.Message}");
        }
    }
}