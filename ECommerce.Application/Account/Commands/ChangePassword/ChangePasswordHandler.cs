using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ChangePassword;

public sealed class ChangePasswordHandler(
    ICurrentUser currentUser,
    IIdentityService identityService)
    : IRequestHandler<ChangePasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var (success, error) = await identityService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword,
            cancellationToken);

        if (!success)
        {
            return Result<string>.Failure(error!);
        }

        return Result<string>.Success("Password changed successfully.");
    }
}

