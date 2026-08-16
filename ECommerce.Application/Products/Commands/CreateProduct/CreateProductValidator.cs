using FluentValidation;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed class CreateProductValidator
    : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.ProductDescription)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.BrandId)
            .NotEmpty();

        RuleFor(x => x.TypeId)
            .NotEmpty();
    }
}