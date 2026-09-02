using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Basket.Queries.GetBasket;

public sealed class GetBasketHandler(
    IBasketRepository basketRepository,
    ICurrentUser currentUser)
    : IRequestHandler<GetBasketQuery, Result<GetBasketResponse>>
{
    private readonly IBasketRepository _basketRepository = basketRepository;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<GetBasketResponse>> Handle(
        GetBasketQuery request,
        CancellationToken ct)
    {
        var basket = await _basketRepository.GetAsync(
            _currentUser.UserId,
            ct);

        if (basket is null)
            return Result<GetBasketResponse>.Failure(
                "Basket not found.");

        var response = new GetBasketResponse(
            basket.Id,
            basket.Items.ToList(),
            basket.TotalQuantity,
            basket.TotalPrice);

        return Result<GetBasketResponse>.Success(response);
    }
}