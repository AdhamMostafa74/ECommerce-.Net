using FluentValidation;

namespace ECommerce.Application.Basket.Commands.UpdateBasketItemQuantity;

public sealed class UpdateBasketItemQuantityValidator
    : AbstractValidator<UpdateBasketItemQuantityCommand>
{
    public UpdateBasketItemQuantityValidator()
    {
  
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}