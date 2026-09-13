using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using MediatR;

namespace ECommerce.Application.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersQuery(
    PaginationRequest PaginationRequest
) : IRequest<Result<PaginatedResult<GetMyOrdersResponse>>>;