using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public sealed class RoleSeeder(
    RoleManager<IdentityRole<Guid>> roleManager)
    : IDataSeeder
{
    private static readonly string[] Roles =
    [
        "Customer",
        "Admin",
        "SuperAdmin"
    ];

    public int Order => 0;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var roleName in Roles)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var role = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant()
            };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to seed role '{roleName}': {errors}");
            }
        }
    }
}