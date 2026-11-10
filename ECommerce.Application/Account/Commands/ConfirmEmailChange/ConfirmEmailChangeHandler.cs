using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ConfirmEmailChange;

public sealed class ConfirmEmailChangeHandler(
    IIdentityService identityService)
    : IRequestHandler<ConfirmEmailChangeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        ConfirmEmailChangeCommand request,
        CancellationToken cancellationToken)
    {
        var (success, error) = await identityService.ConfirmEmailChangeAsync(
            request.UserId,
            request.NewEmail,
            request.Token,
            cancellationToken);

        if (!success)
        {
            return Result<string>.Failure(error!);
        }

        return Result<string>.Success("Email address has been updated successfully.");
    }
}

