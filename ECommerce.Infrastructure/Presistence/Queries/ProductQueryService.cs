using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class ProductQueryService(IRepository<Product> _repo)
    : IProductQueryService
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetAllProductResponse(
            PaginationRequest pagination,
            CancellationToken ct = default)
    {
        var spec = new ProductsSpecification(pagination);

        var products = await _repo
            .ApplySpecification(spec)
            .ProjectToType<GetAllProductResponse>()
            .ToPaginatedResultAsync(pagination, ct);

        return Result<PaginatedResult<GetAllProductResponse>>
            .Success(products);
    }

    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetDeletedProductResponse(
            PaginationRequest pagination,
            CancellationToken ct = default)
    {
        var spec = new DeletedProductsSpecification(pagination);

        var products = await _repo
            .ApplySpecification(spec)
            .ProjectToType<GetAllProductResponse>()
            .ToPaginatedResultAsync(pagination, ct);

        return Result<PaginatedResult<GetAllProductResponse>>
            .Success(products);
    }

    public async Task<Result<PaginatedResult<GetAllProductResponse>>>
        GetAllProductsIncludingDeletedResponse(
            PaginationRequest pagination,
            CancellationToken ct = default)
    {
        var spec = new AllProductsSpecification(pagination);

        var products = await _repo
            .ApplySpecification(spec)
            .ProjectToType<GetAllProductResponse>()
            .ToPaginatedResultAsync(pagination, ct);

        return Result<PaginatedResult<GetAllProductResponse>>
            .Success(products);
    }

    public async Task<Result<GetByIdProductResponse>>
        GetByIdProductResponse(
            Guid id,
            CancellationToken ct = default)
    {
        var spec = new ProductByIdSpecification(id);

        var product = await _repo
            .ApplySpecification(spec)
            .ProjectToType<GetByIdProductResponse>()
            .FirstOrDefaultAsync(ct);

        if (product is null)
            return Result<GetByIdProductResponse>
                .Failure(ProductErrors.NotFound);

        return Result<GetByIdProductResponse>
            .Success(product);
    }
}