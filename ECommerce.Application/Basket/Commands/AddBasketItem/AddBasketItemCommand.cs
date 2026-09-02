using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Basket.Commands.AddBasketItem;

public sealed record AddBasketItemCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<GetBasketResponse>>;