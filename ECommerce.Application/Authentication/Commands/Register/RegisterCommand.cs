using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string UserName,
    string Password) : IRequest<Result<Guid>>;