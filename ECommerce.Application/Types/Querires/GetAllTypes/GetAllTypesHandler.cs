
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Queries.GetAllTypes;
using ECommerce.Domain.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Querires.GetAllTypes;

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

        var validationResult =
    PaginationValidator.Validate(request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllTypesResponse>>
                .Failure(validationResult.Errors);
        }

        return await typeQueryService.GetAllTypeResponse(
             request.PaginationRequest,
             ct);
    }
}