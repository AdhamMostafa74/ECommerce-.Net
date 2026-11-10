using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.UpdateUserName;

public sealed class UpdateUserNameHandler(
    ICurrentUser currentUser,
    IIdentityService identityService)
    : IRequestHandler<UpdateUserNameCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        UpdateUserNameCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var (success, error) = await identityService.UpdateUserNameAsync(
            userId,
            request.NewUserName,
            cancellationToken);

        if (!success)
        {
            return Result<string>.Failure(error!);
        }

        return Result<string>.Success("Username has been updated successfully.");
    }
}

