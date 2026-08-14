using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetProductById;

public sealed record GetByIdProductsQuery(Guid Id)
    : IRequest<Result<GetByIdProductResponse>>;