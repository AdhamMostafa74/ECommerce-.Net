using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Orders;
using ECommerce.Application.Orders.Errors;
using ECommerce.Application.Orders.Queries.GetMyOrders;
using ECommerce.Application.Orders.Queries.GetOrderDetails;
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

    public async Task<Result<GetOrderDetailsResponse>>
        GetOrderDetails(
            Guid orderId,
            Guid customerId,
            CancellationToken ct = default)
    {
        var spec =
            new OrderByIdAndCustomerSpecification(
                orderId,
                customerId);

        var order = await repo
            .ApplySpecification(spec)
            .AsNoTracking()
            .Select(order => new GetOrderDetailsResponse(
                order.Id,
                order.Status,
                order.TotalPrice,
                order.TotalQuantity,
                order.CreatedAt,
                new ShippingAddressResponse(
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.PhoneNumber,
                    order.ShippingAddress.Street,
                    order.ShippingAddress.City,
                    order.ShippingAddress.State,
                    order.ShippingAddress.PostalCode,
                    order.ShippingAddress.Country),
                order.Items
                    .Select(item => new OrderItemResponse(
                        item.ProductId,
                        item.ProductName,
                        item.PictureUrl,
                        item.UnitPrice,
                        item.Quantity,
                        item.TotalPrice))
                    .ToList()))
            .FirstOrDefaultAsync(ct);

        if (order is null)
        {
            return Result<GetOrderDetailsResponse>
                .Failure(
                    OrderErrors.NotFound);
        }

        return Result<GetOrderDetailsResponse>
            .Success(order);
    }
}