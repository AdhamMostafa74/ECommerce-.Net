using FluentValidation;

namespace ECommerce.Application.Account.Commands.ForgotPassword;

public sealed class ForgotPasswordValidator
    : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}

