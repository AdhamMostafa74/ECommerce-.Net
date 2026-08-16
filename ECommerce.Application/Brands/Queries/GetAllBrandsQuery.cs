using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Brands.Queries;

public sealed class GetAllBrandsQuery(
    IBrandQueryService brandQueryService)
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>> ExecuteAsync(PaginationRequest paginationRequest
        , CancellationToken ct)
    {
        var brands = await brandQueryService.GetAllBrandResponse(paginationRequest, ct);

        return (brands);
    }
}