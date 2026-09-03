using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProductsIncludingDeleted;

public sealed class GetAllProductsIncludingDeletedHandler(
    IProductQueryService productQueryService)
    : IRequestHandler<
        GetAllProductsIncludingDeletedQuery,
        Result<PaginatedResult<GetAllProductResponse>>>
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        Handle(
            GetAllProductsIncludingDeletedQuery request,
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
            .GetAllProductsIncludingDeletedResponse(
                request.PaginationRequest,
                ct);
    }
}