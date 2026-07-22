using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.Common;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class ProductQueryService(ECommerceDbContext db) : IProductQueryService
{
    public async Task<Result<PaginatedResult<GetAllProductResponse>>> GetAllProductResponse(
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var products = await db.Products
            .AsNoTracking()
            .ProjectToType<GetAllProductResponse>()
            .ToPaginatedResultAsync(pagination, ct);

        return Result<PaginatedResult<GetAllProductResponse>>.Success(products);
    }

    public async Task<Result<GetByIdProductResponse>> GetByIdProductResponse(
        Guid id,
        CancellationToken ct = default)
    {
        var product = await db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .ProjectToType<GetByIdProductResponse>()
            .FirstOrDefaultAsync(ct);

        if (product is null)
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);

        return Result<GetByIdProductResponse>.Success(product);
    }
}