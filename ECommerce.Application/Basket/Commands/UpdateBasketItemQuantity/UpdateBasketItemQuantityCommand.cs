using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Basket.Commands.UpdateBasketItemQuantity;

public sealed record UpdateBasketItemQuantityCommand(
    Guid ProductId,
    int Quantity
) : IRequest<Result<GetBasketResponse>>;