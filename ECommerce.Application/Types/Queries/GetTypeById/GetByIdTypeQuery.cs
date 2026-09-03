using ECommerce.Application.Types.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Types.Queries.GetTypeById;

public sealed record GetTypeByIdQuery(Guid Id)
    : IRequest<Result<GetByIdTypeResponse>>;