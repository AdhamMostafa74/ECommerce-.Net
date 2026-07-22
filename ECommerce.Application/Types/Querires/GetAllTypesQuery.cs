using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Types.Queries;

public sealed class GetAllTypesQuery(
    ITypeQueryService typeQueryService)
{
    public async Task<Result<PaginatedResult<GetAllTypesResponse>>> ExecuteAsync( PaginationRequest paginationRequest 
        , CancellationToken ct)
    {
        var types = await typeQueryService.GetAllTypeResponse(paginationRequest, ct);

        return types;
    }
}