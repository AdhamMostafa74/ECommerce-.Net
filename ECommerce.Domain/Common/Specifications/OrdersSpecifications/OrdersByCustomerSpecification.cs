using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Domain.Common.Specifications.OrdersSpecifications;

public sealed class OrdersByCustomerSpecification
    : BaseSpecification<Order>
    {
    public OrdersByCustomerSpecification(Guid customerId)
        {
        AddCriteria(order =>
            order.CustomerId == customerId &&
            !order.IsDeleted);

        ApplyOrderByDescending(order =>
            order.CreatedAt);
        }
    }