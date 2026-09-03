using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetAllBrandsIncludingDeleted;

public sealed class GetAllBrandsIncludingDeletedHandler(
    IBrandQueryService brandQueryService)
    : IRequestHandler<
        GetAllBrandsIncludingDeletedQuery,
        Result<PaginatedResult<GetAllBrandsResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>>
        Handle(
            GetAllBrandsIncludingDeletedQuery request,
            CancellationToken ct)
    {
        var validationResult =
            PaginationValidator.Validate(
                request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllBrandsResponse>>
                .Failure(validationResult.Errors);
        }

        return await brandQueryService
            .GetAllBrandsIncludingDeletedResponse(
                request.PaginationRequest,
                ct);
    }
}