using FluentValidation;

namespace ECommerce.Application.Brands.Commands.CreateBrand;

public sealed class CreateBrandValidator
    : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Brand name is required.");
    }
}