using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersResponse(
    Guid OrderId,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    decimal TotalPrice,
    int TotalQuantity,
    DateTimeOffset CreatedAt);