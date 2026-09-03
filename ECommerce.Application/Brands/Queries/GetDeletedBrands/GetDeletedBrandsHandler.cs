using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetDeletedBrands;

public sealed class GetDeletedBrandsHandler(
    IBrandQueryService brandQueryService)
    : IRequestHandler<
        GetDeletedBrandsQuery,
        Result<PaginatedResult<GetAllBrandsResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>>
        Handle(
            GetDeletedBrandsQuery request,
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
            .GetDeletedBrandResponse(
                request.PaginationRequest,
                ct);
    }
}