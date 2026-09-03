using ECommerce.Application.Common.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.Infrastructure.Identity;

public sealed class CurrentUser(
    IHttpContextAccessor httpContextAccessor)
    : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User;
            var userId = user?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var id))
                throw new UnauthorizedAccessException(
                    "Current user ID is missing or invalid.");

            return id;
        }
    }
}

