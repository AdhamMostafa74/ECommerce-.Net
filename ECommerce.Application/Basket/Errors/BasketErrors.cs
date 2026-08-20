using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Basket.Errors;

public static class BasketErrors
{
    public static readonly Error InvalidBasketId = new(
        "BasketEntity.Invalid Id",
        "The basket ID is invalid.",
        ErrorType.Validation);

    public static readonly Error InvalidProductId = new(
        "BasketEntity.Invalid Product Id",
        "The product ID is invalid.",
        ErrorType.Validation);

    public static readonly Error InvalidQuantity = new(
        "BasketEntity.Invalid Quantity",
        "Quantity must be greater than zero.",
        ErrorType.Validation);

    public static readonly Error ItemNotFound = new(
        "BasketEntity.Item Not Found",
        "The requested product is not in the basket.",
        ErrorType.NotFound);

    public static readonly Error EmptyBasket = new(
        "BasketEntity.Empty",
        "The basket is empty.",
        ErrorType.Failure);

    public static readonly Error NotFound = new(
    "Basket.NotFound",
    "The requested basket was not found.",
    ErrorType.NotFound);
}