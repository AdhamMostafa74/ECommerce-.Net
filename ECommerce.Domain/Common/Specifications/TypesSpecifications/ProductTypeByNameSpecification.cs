using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.TypesSpecifications;

public sealed class ProductTypeByNameSpecification
    : BaseSpecification<ProductType>
{
    public ProductTypeByNameSpecification(string name)
    {
        AddCriteria(t =>
            t.Name == name &&
            !t.IsDeleted);
    }

    public ProductTypeByNameSpecification(
        string name,
        Guid excludedTypeId)
    {
        AddCriteria(t =>
            t.Name == name &&
            t.Id != excludedTypeId &&
            !t.IsDeleted);
    }
}