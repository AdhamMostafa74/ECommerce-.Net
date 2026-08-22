using FluentValidation;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed class CreateProductValidator
    : AbstractValidator<CreateProductCommand>
{
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

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
            .Must(picture => picture!.Content.Length <= MaxFileSize)
            .WithMessage("Picture size must not exceed 5 MB.")
            .When(x => x.Picture is not null);

        RuleFor(x => x.Picture)
            .Must(picture =>
                AllowedContentTypes.Contains(
                    picture!.ContentType,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage("Only JPEG, PNG, and WebP images are allowed.")
            .When(x => x.Picture is not null);

        RuleFor(x => x.BrandId)
            .NotEmpty();

        RuleFor(x => x.TypeId)
            .NotEmpty();
    }
}