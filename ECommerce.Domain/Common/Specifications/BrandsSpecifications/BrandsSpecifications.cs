using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.BrandsSpecifications;

public sealed class BrandsSpecification
    : BaseSpecification<ProductBrand>
{
    public BrandsSpecification()
    {
        AddCriteria(b => !b.IsDeleted);

        ApplyOrderBy(b => b.Name);
    }
}