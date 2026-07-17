using ECommerce.Application.Brands;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Infrastructure.Data.DbContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class BrandQueryService(ECommerceDbContext db) : IBrandQueryService
{
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandResponse(
        CancellationToken ct = default)
    {
        return await db.ProductBrands
            .ProjectToType<GetAllBrandsResponse>()
            .ToListAsync(ct);
    }

    public async Task<GetByIdBrandResponse?> GetByIdBrandResponse(
        Guid id,
        CancellationToken ct = default)
    {
        return await db.ProductBrands
            .Where(x => x.Id == id)
            .ProjectToType<GetByIdBrandResponse>()
            .FirstOrDefaultAsync( ct);
    }
}