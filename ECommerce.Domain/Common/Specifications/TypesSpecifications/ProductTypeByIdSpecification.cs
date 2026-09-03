using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.TypesSpecifications;

public sealed class ProductTypeByIdSpecification
    : BaseSpecification<ProductType>
{
    public ProductTypeByIdSpecification(Guid id)
    {
        AddCriteria(t =>
            t.Id == id &&
            !t.IsDeleted);
    }
}