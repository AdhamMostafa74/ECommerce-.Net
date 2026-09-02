using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Basket.Commands.ClearBasket;

public sealed record ClearBasketCommand
    : IRequest<Result<GetBasketResponse>>;