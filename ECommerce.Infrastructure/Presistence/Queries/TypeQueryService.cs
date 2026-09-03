using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types;
using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class TypeQueryService(IRepository<ProductType> repo)
    : ITypeQueryService
{
    public async Task<
        Result<PaginatedResult<GetAllTypesResponse>>>
        GetAllTypeResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new ProductTypesSpecification();

        var types = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToPaginatedResultAsync(
                paginationRequest,
                ct);

        return Result<PaginatedResult<GetAllTypesResponse>>
            .Success(types);
    }

    public async Task<
        Result<PaginatedResult<GetAllTypesResponse>>>
        GetDeletedTypeResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new DeletedProductTypesSpecification();

        var types = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToPaginatedResultAsync(
                paginationRequest,
                ct);

        return Result<PaginatedResult<GetAllTypesResponse>>
            .Success(types);
    }

    public async Task<
        Result<PaginatedResult<GetAllTypesResponse>>>
        GetAllTypesIncludingDeletedResponse(
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
    {
        var spec = new AllProductTypesSpecification();

        var types = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetAllTypesResponse>()
            .ToPaginatedResultAsync(
                paginationRequest,
                ct);

        return Result<PaginatedResult<GetAllTypesResponse>>
            .Success(types);
    }

    public async Task<Result<GetByIdTypeResponse>>
        GetByIdTypeResponse(
            Guid id,
            CancellationToken ct = default)
    {
        var spec = new ProductTypeByIdSpecification(id);

        var type = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .ProjectToType<GetByIdTypeResponse>()
            .FirstOrDefaultAsync(ct);

        if (type is null)
        {
            return Result<GetByIdTypeResponse>
                .Failure(TypeErrors.NotFound);
        }

        return Result<GetByIdTypeResponse>
            .Success(type);
    }
}