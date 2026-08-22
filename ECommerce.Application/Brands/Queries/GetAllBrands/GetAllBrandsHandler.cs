using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetAllBrands;

public sealed class GetAllBrandsHandler(
    IBrandQueryService brandQueryService)
    : IRequestHandler<
        GetAllBrandsQuery,
        Result<PaginatedResult<GetAllBrandsResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllBrandsResponse>>> Handle(
        GetAllBrandsQuery request,
        CancellationToken ct)
    {
        var validationResult =
    PaginationValidator.Validate(request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllBrandsResponse>>
                .Failure(validationResult.Error);
        }

        return await brandQueryService.GetAllBrandResponse(
            request.PaginationRequest,
            ct);
    }
}