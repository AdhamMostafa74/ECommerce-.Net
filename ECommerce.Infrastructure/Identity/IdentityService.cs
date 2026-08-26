using ECommerce.Application.Authentication.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

public sealed class IdentityService(
    UserManager<ApplicationUser> userManager)
    : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<(bool Success, Guid UserId)> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return (false, Guid.Empty);

        var validPassword = await _userManager.CheckPasswordAsync(
            user,
            password);

        if (!validPassword)
            return (false, Guid.Empty);

        return (true, user.Id);
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
            return [];

        var roles = await _userManager.GetRolesAsync(user);

        return roles.ToList();
    }

    public async Task<(bool Success, Guid UserId, Error? Error)> CreateUserAsync(
        string email,
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = MapIdentityError(result.Errors);

            return (false, Guid.Empty, error);
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user,
            "Customer");

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return (
                false,
                Guid.Empty,
                AuthenticationErrors.RegistrationFailed);
        }

        return (true, user.Id, null);
    }

    private static Error MapIdentityError(
        IEnumerable<IdentityError> errors)
    {
        var errorList = errors.ToList();

        if (errorList.Any(error =>
            error.Code == "DuplicateUserName"))
        {
            return AuthenticationErrors.UsernameAlreadyExists;
        }

        if (errorList.Any(error =>
            error.Code == "DuplicateEmail"))
        {
            return AuthenticationErrors.EmailAlreadyExists;
        }

        return AuthenticationErrors.InvalidRegistrationData;
    }
}