using ECommerce.Application.Types;
using ECommerce.Application.Types.DTOs;
using ECommerce.Infrastructure.Data.DbContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Presistence.Queries;

public class TypeQueryService(ECommerceDbContext db) : ITypeQueryService
{
    public async Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypeResponse(
        CancellationToken ct = default)
    {
        return await db.ProductTypes
            .ProjectToType<GetAllTypesResponse>()
            .ToListAsync(ct);
    }

    public async Task<GetByIdTypeResponse?> GetByIdTypeResponse(
        Guid id,
        CancellationToken ct = default)
    {
        return await db.ProductTypes
            .Where(x => x.Id == id)
            .ProjectToType<GetByIdTypeResponse>()
            .FirstOrDefaultAsync( ct);
    }
}
