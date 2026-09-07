using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.CustomersSpecifications;

public sealed class CustomerByApplicationUserIdSpecification
    : BaseSpecification<Customer>
{
    public CustomerByApplicationUserIdSpecification(
        Guid applicationUserId)
    {
        AddCriteria(customer =>
            customer.ApplicationUserId == applicationUserId);
    }
}