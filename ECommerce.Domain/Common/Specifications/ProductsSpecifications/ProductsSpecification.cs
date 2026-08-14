using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Specifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Specifications.ProductsSpecifications;

public sealed class ProductsSpecification : BaseSpecification<Product>
{
    public ProductsSpecification(PaginationRequest pagination)
    {
        AddInclude(p => p.ProductBrand);
        AddInclude(p => p.ProductType);

        ApplyOrderBy(p => p.Name);

        ApplyPaging(
            (pagination.PageNumber - 1) * pagination.PageSize,
            pagination.PageSize);
    }
}