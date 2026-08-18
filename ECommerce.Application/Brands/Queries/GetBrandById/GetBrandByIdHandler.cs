using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetBrandById;

public sealed class GetBrandByIdHandler(
    IBrandQueryService brandQueryService)
    : IRequestHandler<
        GetBrandByIdQuery,
        Result<GetByIdBrandResponse>>
{
    public async Task<Result<GetByIdBrandResponse>> Handle(
        GetBrandByIdQuery request,
        CancellationToken ct)
    {
        return await brandQueryService.GetByIdBrandResponse(
            request.Id,
            ct);
    }
}