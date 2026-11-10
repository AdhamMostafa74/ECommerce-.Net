using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ChangeEmail;

public sealed record ChangeEmailCommand(string NewEmail)
    : IRequest<Result<string>>;

