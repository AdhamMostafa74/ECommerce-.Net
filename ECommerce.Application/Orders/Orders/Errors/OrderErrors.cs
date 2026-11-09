using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Orders.Errors;

public static class OrderErrors
{
    public static readonly Error EmptyBasket = new(
        "Order.EmptyBasket",
        "Cannot create an order from an empty basket.",
        ErrorType.Failure);

    public static readonly Error CustomerUnavailable = new(
        "Order.CustomerUnavailable",
        "The customer profile is unavailable.",
        ErrorType.Conflict);

    public static readonly Error InvalidBasket = new(
        "Order.InvalidBasket",
        "The basket contains invalid item data.",
        ErrorType.Failure);

    public static readonly Error NotFound =
    new(
        "Order.NotFound",
        "Order was not found.",
        ErrorType.NotFound);

    public static readonly Error PaymentFailed = new(
        "Order.PaymentFailed",
        "Failed to initiate payment with the payment provider.",
        ErrorType.Failure);

    public static readonly Error PaymentPersistenceFailed = new(
        "Order.PaymentPersistenceFailed",
        "Failed to record payment details for the order.",
        ErrorType.Failure);
}