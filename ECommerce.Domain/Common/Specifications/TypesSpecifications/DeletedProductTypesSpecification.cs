using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.TypesSpecifications;

public sealed class DeletedProductTypesSpecification
    : BaseSpecification<ProductType>
{
    public DeletedProductTypesSpecification()
    {
        AddCriteria(t => t.IsDeleted);

        ApplyOrderBy(t => t.Name);
    }
}