using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.TypesSpecifications;

public sealed class ProductTypesSpecification : BaseSpecification<ProductType>
{
    public ProductTypesSpecification()
    {
        ApplyOrderBy(t => t.Name);
    }
}