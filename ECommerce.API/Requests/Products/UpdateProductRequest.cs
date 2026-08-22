using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Requests.Products;

public sealed class UpdateProductRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public IFormFile? Picture { get; set; }

    public string? Price { get; set; }

    public string? BrandId { get; set; }

    public string? TypeId { get; set; }
}