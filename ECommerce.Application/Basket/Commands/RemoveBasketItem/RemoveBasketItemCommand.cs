using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Basket.Commands.RemoveBasketItem;

public sealed record RemoveBasketItemCommand(
    Guid ProductId
) : IRequest<Result<GetBasketResponse>>;