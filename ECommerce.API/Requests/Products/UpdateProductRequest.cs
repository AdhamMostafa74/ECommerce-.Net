namespace ECommerce.API.Requests.Products;

public sealed record UpdateProductRequest(
    string? Name,
    string? Description,
    decimal? Price,
    Guid? BrandId,
    Guid? TypeId);