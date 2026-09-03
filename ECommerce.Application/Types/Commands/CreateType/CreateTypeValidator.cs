using FluentValidation;

namespace ECommerce.Application.Types.Commands.CreateType;

public sealed class CreateTypeValidator
    : AbstractValidator<CreateTypeCommand>
{
    public CreateTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Type name is required.");
    }
}