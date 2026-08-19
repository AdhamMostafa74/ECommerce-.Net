using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Basket.Commands.AddBasketItem
{
    public sealed record AddBasketItemCommand(
        Guid BasketId,
        Guid ProductId,
        int Quantity
    ) : IRequest<Result>;
}