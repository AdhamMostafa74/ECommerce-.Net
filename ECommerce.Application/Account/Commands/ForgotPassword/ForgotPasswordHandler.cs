using ECommerce.Application.Common.Email;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ForgotPassword;

public sealed class ForgotPasswordHandler(
    IIdentityService identityService,
    IEmailService emailService)
    : IRequestHandler<ForgotPasswordCommand, Result<string>>
{
    private const string GenericResponseMessage =
        "If an account with that email exists, a password reset link has been sent.";

    public async Task<Result<string>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var token = await identityService.GeneratePasswordResetTokenAsync(
            request.Email,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(token))
        {
            await emailService.SendPasswordResetEmailAsync(
                request.Email,
                token,
                cancellationToken);
        }

        return Result<string>.Success(GenericResponseMessage);
    }
}

