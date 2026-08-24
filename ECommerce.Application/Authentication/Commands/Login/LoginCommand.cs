using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Common.Identity.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<string>>;