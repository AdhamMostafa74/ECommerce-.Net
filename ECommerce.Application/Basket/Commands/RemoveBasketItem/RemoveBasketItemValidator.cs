using FluentValidation;

namespace ECommerce.Application.Basket.Commands.RemoveBasketItem;

public sealed class RemoveBasketItemValidator
    : AbstractValidator<RemoveBasketItemCommand>
{
    public RemoveBasketItemValidator()
    {
    

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");
    }
}