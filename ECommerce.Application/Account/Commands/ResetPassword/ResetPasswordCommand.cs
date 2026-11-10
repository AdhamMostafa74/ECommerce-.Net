using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword)
    : IRequest<Result<string>>;

