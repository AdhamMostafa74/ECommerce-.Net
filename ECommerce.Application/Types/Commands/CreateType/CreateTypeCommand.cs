using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Commands.CreateType;

public sealed record CreateTypeCommand(string Name)
    : IRequest<Result<GetAllTypesResponse>>;