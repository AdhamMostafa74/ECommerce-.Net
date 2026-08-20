using FluentValidation;

namespace ECommerce.Application.Basket.Commands.AddBasketItem;

public sealed class AddBasketItemValidator
    : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemValidator()
    {
        RuleFor(x => x.BasketId)
            .NotEmpty()
            .WithMessage("BasketEntity ID is required.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}