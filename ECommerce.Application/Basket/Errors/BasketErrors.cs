using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Basket.Errors;

public static class BasketErrors
{
    public static readonly Error InvalidBasketId = new(
        "Basket.Invalid Id",
        "The basket ID is invalid.",
        ErrorType.Validation);

    public static readonly Error InvalidProductId = new(
        "Basket.Invalid Product Id",
        "The product ID is invalid.",
        ErrorType.Validation);

    public static readonly Error InvalidQuantity = new(
        "Basket.Invalid Quantity",
        "Quantity must be greater than zero.",
        ErrorType.Validation);

    public static readonly Error ItemNotFound = new(
        "Basket.Item Not Found",
        "The requested product is not in the basket.",
        ErrorType.NotFound);

    public static readonly Error EmptyBasket = new(
        "Basket.Empty",
        "The basket is empty.",
        ErrorType.Failure);
}