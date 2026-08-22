using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Requests.Products;

public sealed class UpdateProductRequest
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public IFormFile? Picture { get; set; }

    public decimal? Price { get; set; }

    public Guid? BrandId { get; set; }

    public Guid? TypeId { get; set; }
}