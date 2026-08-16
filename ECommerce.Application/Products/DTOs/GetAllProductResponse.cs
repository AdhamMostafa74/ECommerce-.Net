namespace ECommerce.Application.Products.DTOs;

public record GetAllProductResponse(
    Guid Id,
    string Name,
    string Description, 
    string ProductType, 
    string ProductBrand,
    string PicUrl,
    decimal Price
    )
{
}
