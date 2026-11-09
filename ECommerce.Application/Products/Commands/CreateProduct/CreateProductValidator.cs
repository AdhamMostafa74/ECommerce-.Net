using ECommerce.Application.Common.Files;
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

        RuleFor(x => x.Picture)
            .NotNull()
            .WithMessage("Product picture is required.");

        RuleFor(x => x.Picture)
            .Must(ImageFileValidator.IsValid)
            .WithMessage(
                "Invalid image. Only JPEG, PNG, and WebP images up to 5 MB are allowed.")
            .When(x => x.Picture is not null);

        RuleFor(x => x.BrandId)
            .NotEmpty();

        RuleFor(x => x.TypeId)
            .NotEmpty();
    }
}