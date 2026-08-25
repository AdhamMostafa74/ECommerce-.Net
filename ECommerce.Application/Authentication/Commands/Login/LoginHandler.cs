using ECommerce.Application.Authentication.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Login;

public sealed class LoginHandler(
    IIdentityService identityService,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IIdentityService _identityService = identityService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task<Result<string>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var credentials = await _identityService.ValidateCredentialsAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (!credentials.Success)
            return Result<string>.Failure(
                AuthenticationErrors.InvalidCredentials);

        var roles = await _identityService.GetRolesAsync(
            credentials.UserId,
            cancellationToken);

        var token = _jwtTokenService.GenerateToken(
            credentials.UserId,
            request.Email,
            roles);

        return Result<string>.Success(token);
    }
}