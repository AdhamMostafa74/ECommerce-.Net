using ECommerce.Application.Basket.Errors;
using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Basket.Commands.ClearBasket;

public sealed class ClearBasketHandler(
    IBasketRepository basketRepository,
    ICurrentUser currentUser)
    : IRequestHandler<ClearBasketCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(
        ClearBasketCommand request,
        CancellationToken ct)
    {
        var basket = await basketRepository.GetAsync(
            currentUser.UserId,
            ct);

        if (basket is null)
            return Result<GetBasketResponse>.Failure(
                BasketErrors.NotFound);

        basket.Clear();

        await basketRepository.DeleteAsync(
            currentUser.UserId,
            ct);

        var response = new GetBasketResponse(
            basket.Id,
            [],
            basket.TotalQuantity,
            basket.TotalPrice);

        return Result<GetBasketResponse>.Success(response);
    }
}