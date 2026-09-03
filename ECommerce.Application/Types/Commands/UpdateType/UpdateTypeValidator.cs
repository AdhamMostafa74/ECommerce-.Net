using FluentValidation;

namespace ECommerce.Application.Types.Commands.UpdateType;

public sealed class UpdateTypeValidator
    : AbstractValidator<UpdateTypeCommand>
{
    public UpdateTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Type Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Type name is required.");
    }
}