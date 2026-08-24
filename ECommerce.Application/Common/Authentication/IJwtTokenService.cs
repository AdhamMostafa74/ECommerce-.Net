namespace ECommerce.Application.Common.Identity;

public interface IJwtTokenService
{
    string GenerateToken(
        Guid userId,
        string email,
        IEnumerable<string> roles);
}