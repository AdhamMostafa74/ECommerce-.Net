using FluentValidation;

namespace ECommerce.Application.Account.Commands.UpdateUserName;

public sealed class UpdateUserNameValidator
    : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUserNameValidator()
    {
        RuleFor(x => x.NewUserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z0-9._-]+$")
            .WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens.");
    }
}

