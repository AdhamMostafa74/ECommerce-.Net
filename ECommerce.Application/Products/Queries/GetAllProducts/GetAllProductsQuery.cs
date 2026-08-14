using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        ExecuteAsync(PaginationRequest paginationRequest, CancellationToken ct)
    {

        var products = await productQueryService.GetAllProductResponse(paginationRequest, ct);
        return products;
    }
}
