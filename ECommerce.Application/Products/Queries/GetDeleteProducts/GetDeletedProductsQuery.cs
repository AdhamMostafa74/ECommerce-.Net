using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetDeletedProducts;

public sealed record GetDeletedProductsQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetAllProductResponse>>>;