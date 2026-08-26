using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Register;

public sealed class RegisterHandler(
    IIdentityService identityService)
    : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<Result<Guid>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(
            request.Email,
            request.UserName,
            request.Password,
            cancellationToken);

        if (!result.Success)
        {
            return Result<Guid>.Failure(
                result.Error!);
        }

        return Result<Guid>.Success(result.UserId);
    }
}