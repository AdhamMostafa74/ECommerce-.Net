

using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Products.Queries;

public sealed class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllProductResponse>>> ExecuteAsync() {

        var products = await productQueryService.GetAllProductResponse();
        return Result<IReadOnlyList<GetAllProductResponse>>.Success(products);
    }
}
