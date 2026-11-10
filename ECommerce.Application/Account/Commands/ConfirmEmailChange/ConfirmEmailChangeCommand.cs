using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ConfirmEmailChange;

public sealed record ConfirmEmailChangeCommand(
    Guid UserId,
    string NewEmail,
    string Token)
    : IRequest<Result<string>>;

