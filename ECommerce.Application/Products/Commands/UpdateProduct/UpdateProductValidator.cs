using FluentValidation;

namespace ECommerce.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly string[] AllowedContentTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x)
            .Must(HaveAtLeastOneFieldToUpdate)
            .WithMessage("At least one field must be provided for update.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000)
            .When(x => x.Description is not null);

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

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue);

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .When(x => x.BrandId.HasValue);

        RuleFor(x => x.TypeId)
            .NotEmpty()
            .When(x => x.TypeId.HasValue);
    }

    private static bool HaveAtLeastOneFieldToUpdate(
        UpdateProductCommand command)
    {
        return command.Name is not null ||
               command.Description is not null ||
               command.Picture is not null ||
               command.Price.HasValue ||
               command.BrandId.HasValue ||
               command.TypeId.HasValue;
    }
}