using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Entities.Basket;
using MediatR;

namespace ECommerce.Application.Basket.Queries.GetBasket;

public sealed record GetBasketQuery
    : IRequest<Result<GetBasketResponse>>;