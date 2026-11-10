using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email)
    : IRequest<Result<string>>;

