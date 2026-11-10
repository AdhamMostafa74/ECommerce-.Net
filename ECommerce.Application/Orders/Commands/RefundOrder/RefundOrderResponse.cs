using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Orders.Commands.RefundOrder;

public sealed record RefundOrderResponse(
    Guid OrderId,
    string RefundTransactionId,
    decimal RefundedAmount,
    PaymentStatus PaymentStatus,
    OrderStatus OrderStatus);

