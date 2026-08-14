using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

public sealed class ProductNameSpecification : BaseSpecification<Product>
{
    public ProductNameSpecification(string name)
    {
        AddCriteria(p => p.Name == name);
    }

    public ProductNameSpecification(string name, Guid excludedProductId)
    {
        AddCriteria(p =>
            p.Name == name &&
            p.Id != excludedProductId);
    }
}