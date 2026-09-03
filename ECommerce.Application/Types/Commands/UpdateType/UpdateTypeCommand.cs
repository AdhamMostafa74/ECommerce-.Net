using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Commands.UpdateType;

public sealed record UpdateTypeCommand(
    Guid Id,
    string Name)
    : IRequest<Result<GetAllTypesResponse>>;