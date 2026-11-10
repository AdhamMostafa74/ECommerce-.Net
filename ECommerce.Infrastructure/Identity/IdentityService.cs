
using ECommerce.Application.Account.Errors;
using ECommerce.Application.Authentication.Errors;
using ECommerce.Application.Common.Identity;
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;
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

    public async Task<string?> GetEmailAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        return user?.Email;
    }

    public async Task<(string Email, string UserName)?> GetUserBasicInfoAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null ||
            string.IsNullOrWhiteSpace(user.Email) ||
            string.IsNullOrWhiteSpace(user.UserName))
        {
            return null;
        }

        return (user.Email, user.UserName);
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

    public async Task<string?> GetSecurityStampAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return null;
        }

        return await _userManager.GetSecurityStampAsync(user);
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

        var result = await _userManager.CreateAsync(
            user,
            password);

        if (!result.Succeeded)
        {
            var error = MapIdentityError(result.Errors);

            return (
                false,
                Guid.Empty,
                error);
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

        return (
            true,
            user.Id,
            null);
    }

    public async Task<bool> DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
            return true;

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }

    public async Task<(bool Success, Error? Error)> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return (false, AccountErrors.UserNotFound);
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);

        if (!result.Succeeded)
        {
            var errorList = result.Errors.ToList();

            if (errorList.Any(e => e.Code == "PasswordMismatch"))
            {
                return (false, AccountErrors.InvalidCurrentPassword);
            }

            var passwordError = errorList.FirstOrDefault(e =>
                e.Code.StartsWith("Password", StringComparison.OrdinalIgnoreCase));

            if (passwordError is not null)
            {
                return (
                    false,
                    new Error(
                        "Account.WeakPassword",
                        passwordError.Description,
                        ErrorType.Validation));
            }

            return (false, AccountErrors.PasswordChangeFailed);
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return (true, null);
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<(bool Success, Error? Error)> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return (false, AccountErrors.InvalidOrExpiredResetToken);
        }

        var normalizedToken = token.Contains('%')
            ? Uri.UnescapeDataString(token)
            : token;

        var result = await _userManager.ResetPasswordAsync(
            user,
            normalizedToken,
            newPassword);

        if (!result.Succeeded)
        {
            var errorList = result.Errors.ToList();

            if (errorList.Any(e => e.Code == "InvalidToken"))
            {
                return (false, AccountErrors.InvalidOrExpiredResetToken);
            }

            var passwordError = errorList.FirstOrDefault(e =>
                e.Code.StartsWith("Password", StringComparison.OrdinalIgnoreCase));

            if (passwordError is not null)
            {
                return (
                    false,
                    new Error(
                        "Account.WeakPassword",
                        passwordError.Description,
                        ErrorType.Validation));
            }

            return (false, AccountErrors.PasswordResetFailed);
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return (true, null);
    }

    public async Task<(bool Success, string? Token, Error? Error)> GenerateChangeEmailTokenAsync(
        Guid userId,
        string newEmail,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return (false, null, AccountErrors.UserNotFound);
        }

        var normalizedNewEmail = newEmail.Trim();

        if (string.Equals(
            user.Email,
            normalizedNewEmail,
            StringComparison.OrdinalIgnoreCase))
        {
            return (false, null, AccountErrors.SameEmail);
        }

        var existingUser = await _userManager.FindByEmailAsync(
            normalizedNewEmail);

        if (existingUser is not null && existingUser.Id != user.Id)
        {
            return (false, null, AuthenticationErrors.EmailAlreadyExists);
        }

        var token = await _userManager.GenerateChangeEmailTokenAsync(
            user,
            normalizedNewEmail);

        return (true, token, null);
    }

    public async Task<(bool Success, Error? Error)> ConfirmEmailChangeAsync(
        Guid userId,
        string newEmail,
        string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return (false, AccountErrors.InvalidOrExpiredEmailChangeToken);
        }

        var normalizedNewEmail = newEmail.Trim();

        var existingUser = await _userManager.FindByEmailAsync(
            normalizedNewEmail);

        if (existingUser is not null && existingUser.Id != user.Id)
        {
            return (false, AuthenticationErrors.EmailAlreadyExists);
        }

        var normalizedToken = token.Contains('%')
            ? Uri.UnescapeDataString(token)
            : token;

        var result = await _userManager.ChangeEmailAsync(
            user,
            normalizedNewEmail,
            normalizedToken);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
            {
                return (false, AuthenticationErrors.EmailAlreadyExists);
            }

            return (false, AccountErrors.InvalidOrExpiredEmailChangeToken);
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return (true, null);
    }

    public async Task<(bool Success, Error? Error)> UpdateUserNameAsync(
        Guid userId,
        string newUserName,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return (false, AccountErrors.UserNotFound);
        }

        var normalizedUserName = newUserName.Trim();

        if (string.Equals(
            user.UserName,
            normalizedUserName,
            StringComparison.OrdinalIgnoreCase))
        {
            return (false, AccountErrors.SameUserName);
        }

        var existingUser = await _userManager.FindByNameAsync(
            normalizedUserName);

        if (existingUser is not null && existingUser.Id != user.Id)
        {
            return (false, AuthenticationErrors.UsernameAlreadyExists);
        }

        var result = await _userManager.SetUserNameAsync(
            user,
            normalizedUserName);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
            {
                return (false, AuthenticationErrors.UsernameAlreadyExists);
            }

            if (result.Errors.Any(e => e.Code == "InvalidUserName"))
            {
                return (false, AccountErrors.InvalidUserName);
            }

            return (false, AccountErrors.UserNameUpdateFailed);
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return (true, null);
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
        if (errorList.Any(error =>
        error.Code == "Authentication.GenderError"))
        {
            return AuthenticationErrors.GenderError;
        }

        var passwordError = errorList.FirstOrDefault(error =>
            error.Code.StartsWith("Password", StringComparison.OrdinalIgnoreCase));

        if (passwordError is not null)
        {
            return new Error(
                "Authentication.WeakPassword",
                passwordError.Description,
                ErrorType.Validation);
        }

        return AuthenticationErrors.InvalidRegistrationData;
    }
}
