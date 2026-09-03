using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Queries.GetTypeById;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Queries.GetTypeById;

public sealed class GetTypeByIdHandler(
    ITypeQueryService typeQueryService)
    : IRequestHandler<
        GetTypeByIdQuery,
        Result<GetByIdTypeResponse>>
{
    public async Task<Result<GetByIdTypeResponse>> Handle(
        GetTypeByIdQuery request,
        CancellationToken ct)
    {
        return await typeQueryService.GetByIdTypeResponse(
            request.Id,
            ct);
    }
}