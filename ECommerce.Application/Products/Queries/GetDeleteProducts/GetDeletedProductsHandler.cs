using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetDeletedProducts;

public sealed class GetDeletedProductsHandler(
    IProductQueryService productQueryService)
    : IRequestHandler<
        GetDeletedProductsQuery,
        Result<PaginatedResult<GetAllProductResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        Handle(
            GetDeletedProductsQuery request,
            CancellationToken ct)
    {
        var validationResult =
            PaginationValidator.Validate(request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllProductResponse>>
                .Failure(validationResult.Errors);
        }

        return await productQueryService
            .GetDeletedProductResponse(
                request.PaginationRequest,
                ct);
    }
}