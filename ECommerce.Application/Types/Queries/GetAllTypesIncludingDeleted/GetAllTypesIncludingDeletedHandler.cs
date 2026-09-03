using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Queries.GetAllTypesIncludingDeleted;

public sealed class GetAllTypesIncludingDeletedHandler(
    ITypeQueryService typeQueryService)
    : IRequestHandler<
        GetAllTypesIncludingDeletedQuery,
        Result<PaginatedResult<GetAllTypesResponse>>>
{
    public async Task<
        Result<PaginatedResult<GetAllTypesResponse>>>
        Handle(
            GetAllTypesIncludingDeletedQuery request,
            CancellationToken ct)
    {
        var validationResult =
            PaginationValidator.Validate(
                request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllTypesResponse>>
                .Failure(validationResult.Errors);
        }

        return await typeQueryService
            .GetAllTypesIncludingDeletedResponse(
                request.PaginationRequest,
                ct);
    }
}