using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Orders.Queries.GetMyOrders;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Orders;

public interface IOrderQueryService
    {
    Task<Result<PaginatedResult<GetMyOrdersResponse>>>
        GetMyOrders(
            Guid customerId,
            PaginationRequest pagination,
            CancellationToken ct = default);
    }