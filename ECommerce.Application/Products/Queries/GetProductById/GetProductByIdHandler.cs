using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetProductById;

public sealed class GetByIdProductsHandler(IProductQueryService productQueryService)
        : IRequestHandler<GetByIdProductsQuery, Result<GetByIdProductResponse>>
{
    private readonly IProductQueryService _productQueryService = productQueryService;

    public async Task<Result<GetByIdProductResponse>> Handle(
        GetByIdProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _productQueryService.GetByIdProductResponse(
            request.Id,
            cancellationToken);
    }
}