using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.TypesSpecifications;

public sealed class AllProductTypesSpecification
    : BaseSpecification<ProductType>
{
    public AllProductTypesSpecification()
    {
        ApplyOrderBy(t => t.Name);
    }
}