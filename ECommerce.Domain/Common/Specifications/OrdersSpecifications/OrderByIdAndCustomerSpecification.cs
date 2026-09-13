using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Domain.Common.Specifications.OrdersSpecifications;

public sealed class OrderByIdAndCustomerSpecification
    : BaseSpecification<Order>
{
    public OrderByIdAndCustomerSpecification(
        Guid orderId,
        Guid customerId)
    {
        AddCriteria(order =>
            order.Id == orderId &&
            order.CustomerId == customerId &&
            !order.IsDeleted);

        AddInclude(order => order.Items);
    }
}