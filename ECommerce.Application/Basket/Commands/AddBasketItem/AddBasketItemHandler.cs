using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Basket;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Basket.Commands.AddBasketItem;

public sealed class AddBasketItemHandler(
    IRepository<Product> productRepository,
    IBasketRepository basketRepository)
    : IRequestHandler<AddBasketItemCommand, Result>
{
    public async Task<Result> Handle(
        AddBasketItemCommand request,
        CancellationToken ct)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.ProductId),
            ct);

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        var basket = await basketRepository.GetAsync(
            request.BasketId,
            ct);

        basket ??= new Domain.Entities.Basket.BasketEntity(request.BasketId);

        var item = new BasketItem(
            product.Id,
            product.Name,
            product.PictureUrl,
            product.Price,
            request.Quantity);

        basket.AddItem(item);

        await basketRepository.SaveAsync(
            basket,
            ct);

        return Result.Success();
    }
}