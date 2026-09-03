using FluentValidation;

namespace ECommerce.Application.Brands.Commands.UpdateBrand;

public sealed class UpdateBrandValidator
    : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Brand ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Brand name is required.");
    }
}