using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Application.Orders.Queries.GetOrderDetails;

public sealed record GetOrderDetailsResponse(
    Guid OrderId,
    OrderStatus Status,
    decimal TotalPrice,
    int TotalQuantity,
    DateTimeOffset CreatedAt,
    ShippingAddressResponse ShippingAddress,
    IReadOnlyList<OrderItemResponse> Items);

public sealed record ShippingAddressResponse(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);

public sealed record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);