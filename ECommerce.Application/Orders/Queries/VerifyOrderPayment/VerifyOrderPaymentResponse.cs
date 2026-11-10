using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Orders.Queries.VerifyOrderPayment;

public sealed record VerifyOrderPaymentResponse(
    Guid OrderId,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    bool IsPaid,
    decimal TotalPrice,
    DateTimeOffset CreatedAt);

