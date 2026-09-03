using ECommerce.Application.Brands;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class BrandQueryService(IRepository<ProductBrand> repo)
    : IBrandQueryService
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>>
        GetAllBrandResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new BrandsSpecification();

        var brands = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToPaginatedResultAsync(paginationRequest, ct);

        return Result<PaginatedResult<GetAllBrandsResponse>>
            .Success(brands);
    }

    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>>
        GetDeletedBrandResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new DeletedBrandsSpecification();

        var brands = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToPaginatedResultAsync(paginationRequest, ct);

        return Result<PaginatedResult<GetAllBrandsResponse>>
            .Success(brands);
    }

    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>>
        GetAllBrandsIncludingDeletedResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new AllBrandsSpecification();

        var brands = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllBrandsResponse>()
            .ToPaginatedResultAsync(paginationRequest, ct);

        return Result<PaginatedResult<GetAllBrandsResponse>>
            .Success(brands);
    }

    public async Task<Result<GetByIdBrandResponse>>
        GetByIdBrandResponse(
            Guid id,
            CancellationToken ct = default)
    {
        var spec = new BrandByIdSpecification(id);

        var brand = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetByIdBrandResponse>()
            .FirstOrDefaultAsync(ct);

        if (brand is null)
        {
            return Result<GetByIdBrandResponse>
                .Failure(BrandErrors.NotFound);
        }

        return Result<GetByIdBrandResponse>
            .Success(brand);
    }
}