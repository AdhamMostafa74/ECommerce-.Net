using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types;
using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.Presistence.Queries;

public class TypeQueryService(ECommerceDbContext db) : ITypeQueryService
{
    public async Task<Result<PaginatedResult<GetAllTypesResponse>>> GetAllTypeResponse(
        PaginationRequest paginationRequest,
        CancellationToken ct = default)
    {
        var response = await db.ProductTypes
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToPaginatedResultAsync(paginationRequest, ct);

        return Result<PaginatedResult<GetAllTypesResponse>>.Success(response);
    }

    public async Task<Result<GetByIdTypeResponse>> GetByIdTypeResponse(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await db.ProductTypes
            .Where(x => x.Id == id)
            .ProjectToType<GetByIdTypeResponse>()
            .FirstOrDefaultAsync(ct);
        return response is null
            ? Result<GetByIdTypeResponse>.Failure(TypeErrors.NotFound)
            : Result<GetByIdTypeResponse>.Success(response);
    }
}
