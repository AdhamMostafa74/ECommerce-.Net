
using ECommerce.Domain.Common;

namespace ECommerce.Application.Common.Identity;

public interface IIdentityService
{
    Task<(bool Success, Guid UserId)> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<string?> GetSecurityStampAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Guid UserId, Error? Error)> CreateUserAsync(
        string email,
        string userName,
        string password,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<string?> GetEmailAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<(string Email, string UserName)?> GetUserBasicInfoAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Error? Error)> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    Task<string?> GeneratePasswordResetTokenAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Error? Error)> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

    Task<(bool Success, string? Token, Error? Error)> GenerateChangeEmailTokenAsync(
        Guid userId,
        string newEmail,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Error? Error)> ConfirmEmailChangeAsync(
        Guid userId,
        string newEmail,
        string token,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Error? Error)> UpdateUserNameAsync(
        Guid userId,
        string newUserName,
        CancellationToken cancellationToken = default);
}
