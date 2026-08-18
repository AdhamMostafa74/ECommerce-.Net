using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Queries.GetAllTypes;

public sealed class GetAllTypesHandler(
    ITypeQueryService typeQueryService)
    : IRequestHandler<
        GetAllTypesQuery,
        Result<PaginatedResult<GetAllTypesResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllTypesResponse>>> Handle(
        GetAllTypesQuery request,
        CancellationToken ct)
    {
        return await typeQueryService.GetAllTypeResponse(
            request.PaginationRequest,
            ct);
    }
}