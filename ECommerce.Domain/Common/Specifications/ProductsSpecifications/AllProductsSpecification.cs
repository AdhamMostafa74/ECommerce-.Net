using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.ProductsSpecifications;

public sealed class AllProductsSpecification
    : BaseSpecification<Product>
{
    public AllProductsSpecification()
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        ApplyOrderBy(p => p.Name);
    }
}
