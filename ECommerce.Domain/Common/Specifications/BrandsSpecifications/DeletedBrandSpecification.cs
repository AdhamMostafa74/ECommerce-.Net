using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.BrandsSpecifications;

public sealed class DeletedBrandsSpecification
    : BaseSpecification<ProductBrand>
{
    public DeletedBrandsSpecification()
    {
        AddCriteria(b => b.IsDeleted);

        ApplyOrderBy(b => b.Name);
    }
}