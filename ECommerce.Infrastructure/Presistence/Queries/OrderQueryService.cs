using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Orders;
using ECommerce.Application.Orders.Queries.GetMyOrders;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.OrdersSpecifications;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Presistence.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public sealed class OrderQueryService(IRepository<Order> repo)
    : IOrderQueryService
    {
    public async Task<
        Result<PaginatedResult<GetMyOrdersResponse>>>
        GetMyOrders(
            Guid customerId,
            PaginationRequest paginationRequest,
            CancellationToken ct = default)
        {
        var spec =
            new OrdersByCustomerSpecification(customerId);

        var orders = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .Select(order => new GetMyOrdersResponse(
                order.Id,
                order.Status,
                order.TotalPrice,
                order.TotalQuantity,
                order.CreatedAt))
            .ToPaginatedResultAsync(
                paginationRequest,
                ct);

        return Result<PaginatedResult<GetMyOrdersResponse>>
            .Success(orders);
        }
    }