namespace ECommerce.Application.Common.Identity;

public interface ICurrentUser
{
    Guid UserId { get; }
}