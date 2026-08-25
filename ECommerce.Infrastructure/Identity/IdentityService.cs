using ECommerce.Application.Common.Identity;
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
    public async Task<(bool Success, Guid UserId, IReadOnlyList<string> Errors)> CreateUserAsync(
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

        if (result.Succeeded)
            return (true, user.Id, []);

        var errors = result.Errors
            .Select(error => error.Description)
            .ToList();

        return (false, Guid.Empty, errors);
    }
}