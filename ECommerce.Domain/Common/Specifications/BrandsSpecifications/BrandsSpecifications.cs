using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.BrandsSpecifications;

public sealed class BrandsSpecification : BaseSpecification<ProductBrand>
{
    public BrandsSpecification()
    {
        ApplyOrderBy(b => b.Name);
    }
}