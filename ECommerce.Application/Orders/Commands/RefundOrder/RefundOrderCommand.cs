using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Orders.Commands.RefundOrder;

public sealed record RefundOrderCommand(
    Guid OrderId,
    decimal? Amount = null)
    : IRequest<Result<RefundOrderResponse>>;

