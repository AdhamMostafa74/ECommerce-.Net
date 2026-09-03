using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Commands.DeleteType;

public sealed record DeleteTypeCommand(Guid Id)
    : IRequest<Result<Unit>>;