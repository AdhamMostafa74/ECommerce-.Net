using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string UserName,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Gender) : IRequest<Result<Guid>>;