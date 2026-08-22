using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsHandler(
    IProductQueryService productQueryService)
    : IRequestHandler<
        GetAllProductsQuery,
        Result<PaginatedResult<GetAllProductResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>> Handle(
     GetAllProductsQuery request,
     CancellationToken ct)
    {
        var validationResult =
            PaginationValidator.Validate(request.PaginationRequest);

        if (validationResult.IsFailure)
        {
            return Result<PaginatedResult<GetAllProductResponse>>
                .Failure(validationResult.Error);
        }

        return await productQueryService.GetAllProductResponse(
            request.PaginationRequest,
            ct);
    }
}