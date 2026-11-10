namespace ECommerce.Application.Account.Queries.GetProfile;

public sealed record ProfileResponse(
    Guid UserId,
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Gender);

