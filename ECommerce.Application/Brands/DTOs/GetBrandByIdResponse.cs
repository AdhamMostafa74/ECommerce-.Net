namespace ECommerce.Application.Brands.DTOs;

public record GetByIdBrandResponse(
    Guid Id,
    string Name
);