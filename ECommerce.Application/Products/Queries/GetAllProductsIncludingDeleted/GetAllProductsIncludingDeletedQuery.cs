using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.DTOs;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Products.Queries.GetAllProductsIncludingDeleted;

public sealed record GetAllProductsIncludingDeletedQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetAllProductResponse>>>;