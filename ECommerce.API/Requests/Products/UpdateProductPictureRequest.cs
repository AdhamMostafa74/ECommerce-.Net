using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Requests.Products;

public sealed class UpdateProductPictureRequest
{
    public IFormFile? Picture { get; set; }
}