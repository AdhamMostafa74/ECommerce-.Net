using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Brands.Queries.GetBrandById;

public sealed record GetBrandByIdQuery(Guid Id)
    : IRequest<Result<GetByIdBrandResponse>>;