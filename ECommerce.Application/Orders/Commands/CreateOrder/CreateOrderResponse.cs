
namespace ECommerce.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderResponse(
    Guid OrderId,
    string? PaymentClientSecret);
