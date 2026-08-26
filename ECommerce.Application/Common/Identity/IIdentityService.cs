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

    Task<(bool Success, Guid UserId, Error? Error)> CreateUserAsync(
        string email,
        string userName,
        string password,
        CancellationToken cancellationToken = default);
}