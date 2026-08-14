using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.BrandsSpecifications;

public sealed class BrandByIdSpecification : BaseSpecification<ProductBrand>
{
    public BrandByIdSpecification(Guid id)
    {
        AddCriteria(b => b.Id == id);
    }
}