using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.ProductsSpecifications;

public sealed class ProductsForOrderCreationSpecification
    : BaseSpecification<Product>
{
    public ProductsForOrderCreationSpecification(
        IReadOnlyCollection<Guid> productIds)
    {
        var ids = productIds.ToArray();

        AddCriteria(product =>
            ids.Contains(product.Id) &&
            !product.IsDeleted);
    }
}