using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.ProductsSpecifications;

public sealed class DeletedProductsSpecification
    : BaseSpecification<Product>
{
    public DeletedProductsSpecification()
    {
        AddCriteria(p => p.IsDeleted);

        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        ApplyOrderBy(p => p.Name);
    }
}
