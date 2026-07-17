

using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Products.Queries;

public sealed class GetByIdProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<GetByIdProductResponse>> ExecuteAsync(Guid id)
    {

        var product = await productQueryService.GetByIdProductResponse(id);
        if(product != null)
            return Result<GetByIdProductResponse>.Success(product);
        else
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);

    }
}
