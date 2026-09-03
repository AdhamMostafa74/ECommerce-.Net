using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.ProductsSpecifications;

public sealed class ProductByIdSpecification : BaseSpecification<Product>
{
    public ProductByIdSpecification(Guid id)
    {
        AddCriteria(p =>
            p.Id == id &&
            !p.IsDeleted);

        AddInclude(p => p.ProductBrand);

        AddInclude(p => p.ProductType);
    }
}