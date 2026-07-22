using ECommerce.Application.Brands;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class BrandQueryService(ECommerceDbContext db) : IBrandQueryService
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>> GetAllBrandResponse(
        PaginationRequest paginationRequest,
        CancellationToken ct = default)
    {
        var brands = await db.ProductBrands
           .AsNoTracking()
.ProjectToType<GetAllBrandsResponse>()
.ToPaginatedResultAsync(paginationRequest, ct);

        return Result<PaginatedResult<GetAllBrandsResponse>>.Success(brands);
    }

    public async Task<Result<GetByIdBrandResponse>> GetByIdBrandResponse(
        Guid id,
        CancellationToken ct = default)
    {
        var brand = await db.ProductBrands
             .Where(x => x.Id == id)
             .ProjectToType<GetByIdBrandResponse>()
             .FirstOrDefaultAsync(ct);


        if (brand is null)
        {
            return Result<GetByIdBrandResponse>.Failure(
                BrandErrors.NotFound);
        }

        return Result<GetByIdBrandResponse>.Success(brand);
    }
}