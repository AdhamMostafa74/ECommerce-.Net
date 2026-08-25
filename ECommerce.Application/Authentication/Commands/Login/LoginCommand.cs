using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Authentication.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<string>>;