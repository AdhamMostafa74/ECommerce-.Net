using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Account.Commands.UpdateUserName;

public sealed record UpdateUserNameCommand(string NewUserName)
    : IRequest<Result<string>>;

