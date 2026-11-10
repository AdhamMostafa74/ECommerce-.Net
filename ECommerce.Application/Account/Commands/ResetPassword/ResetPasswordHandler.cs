using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ResetPassword;

public sealed class ResetPasswordHandler(
    IIdentityService identityService)
    : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var (success, error) = await identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword,
            cancellationToken);

        if (!success)
        {
            return Result<string>.Failure(error!);
        }

        return Result<string>.Success("Password has been reset successfully.");
    }
}

