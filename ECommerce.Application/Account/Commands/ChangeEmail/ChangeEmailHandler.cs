using ECommerce.Application.Common.Email;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ChangeEmail;

public sealed class ChangeEmailHandler(
    ICurrentUser currentUser,
    IIdentityService identityService,
    IEmailService emailService)
    : IRequestHandler<ChangeEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var trimmedEmail = request.NewEmail.Trim();

        var (success, token, error) = await identityService.GenerateChangeEmailTokenAsync(
            userId,
            trimmedEmail,
            cancellationToken);

        if (!success)
        {
            return Result<string>.Failure(error!);
        }

        await emailService.SendEmailChangeConfirmationAsync(
            trimmedEmail,
            userId,
            token!,
            cancellationToken);

        return Result<string>.Success(
            "A confirmation link has been sent to your new email address.");
    }
}

