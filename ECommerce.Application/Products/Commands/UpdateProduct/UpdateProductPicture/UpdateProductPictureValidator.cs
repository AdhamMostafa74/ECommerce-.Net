using ECommerce.Application.Common.Files;
using FluentValidation;

namespace ECommerce.Application.Products.Commands.UpdateProductPicture;

public sealed class UpdateProductPictureValidator
    : AbstractValidator<UpdateProductPictureCommand>
{
    public UpdateProductPictureValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Picture)
            .NotNull()
            .WithMessage("Picture is required.");

        RuleFor(x => x.Picture)
            .Must(ImageFileValidator.IsValid)
            .WithMessage(
                "Invalid image. Only JPEG, PNG, and WebP images up to 5 MB are allowed.")
            .When(x => x.Picture is not null);
    }
}