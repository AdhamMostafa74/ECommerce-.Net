using ECommerce.Application.Account.Queries.GetProfile;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Gender)
    : IRequest<Result<ProfileResponse>>;

