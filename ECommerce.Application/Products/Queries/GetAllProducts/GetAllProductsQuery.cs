using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetAllProductResponse>>>;