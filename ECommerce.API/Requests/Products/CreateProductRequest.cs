using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Requests.Products;

public sealed class CreateProductRequest
{
    public string ProductName { get; set; } = string.Empty;

    public string ProductDescription { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public IFormFile? Picture { get; set; }

    public Guid BrandId { get; set; }

    public Guid TypeId { get; set; }
}