using FluentValidation;

namespace ECommerce.Application.Account.Commands.ConfirmEmailChange;

public sealed class ConfirmEmailChangeValidator
    : AbstractValidator<ConfirmEmailChangeCommand>
{
    public ConfirmEmailChangeValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}

