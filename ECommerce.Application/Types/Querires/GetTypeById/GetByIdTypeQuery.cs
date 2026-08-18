using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Querires.GetTypeById;

public sealed record GetTypeByIdQuery(Guid Id)
    : IRequest<Result<GetByIdTypeResponse>>;