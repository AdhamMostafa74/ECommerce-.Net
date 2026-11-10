using ECommerce.Domain.Entities.Orders;

namespace ECommerce.Domain.Common.Specifications.OrdersSpecifications;

public sealed class OrderWithPaymentsByIdSpecification
    : BaseSpecification<Order>
{
    public OrderWithPaymentsByIdSpecification(Guid orderId)
    {
        AddCriteria(order =>
            order.Id == orderId &&
            !order.IsDeleted);

        AddInclude(order => order.Payments);
        AddInclude(order => order.Items);
    }
}

