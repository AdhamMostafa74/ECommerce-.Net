using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.BrandsSpecifications;

public sealed class BrandByNameSpecification : BaseSpecification<ProductBrand>
{
    public BrandByNameSpecification(string name)
    {
        AddCriteria(b => b.Name == name);
    }

    public BrandByNameSpecification(string name, Guid excludedBrandId)
    {
        AddCriteria(b =>
            b.Name == name &&
            b.Id != excludedBrandId);
    }
}