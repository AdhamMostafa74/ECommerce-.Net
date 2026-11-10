using FluentValidation;

namespace ECommerce.Application.Account.Commands.ChangeEmail;

public sealed class ChangeEmailValidator
    : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}

