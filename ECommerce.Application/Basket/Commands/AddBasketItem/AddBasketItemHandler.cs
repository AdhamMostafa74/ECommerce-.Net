using ECommerce.Application.Basket.Queries.DTOs;
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
    : IRequestHandler<AddBasketItemCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(
        AddBasketItemCommand request,
        CancellationToken ct)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.ProductId),
            ct);

        if (product is null)
            return Result<GetBasketResponse>.Failure(
                ProductErrors.NotFound);

        var basket = await basketRepository.GetAsync(
            request.BasketId,
            ct);

        basket ??= new BasketEntity(request.BasketId);

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

        var response = new GetBasketResponse(
            basket.Id,
            basket.Items.Select(item =>
                new GetBasketItemResponse(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.Price,
                    item.Quantity))
                .ToList(),
            basket.TotalQuantity,
            basket.TotalPrice);

        return Result<GetBasketResponse>.Success(response);
    }
}