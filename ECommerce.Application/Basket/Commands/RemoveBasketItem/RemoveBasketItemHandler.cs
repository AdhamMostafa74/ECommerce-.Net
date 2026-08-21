using ECommerce.Application.Basket.Errors;
using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Basket.Commands.RemoveBasketItem;

public sealed class RemoveBasketItemHandler(
    IBasketRepository basketRepository)
    : IRequestHandler<
        RemoveBasketItemCommand,
        Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(
        RemoveBasketItemCommand request,
        CancellationToken ct)
    {
        var basket = await basketRepository.GetAsync(
            request.BasketId,
            ct);

        if (basket is null)
            return Result<GetBasketResponse>.Failure(
                BasketErrors.NotFound);

        var removed = basket.RemoveItem(
            request.ProductId);

        if (!removed)
            return Result<GetBasketResponse>.Failure(
                BasketErrors.ItemNotFound);

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