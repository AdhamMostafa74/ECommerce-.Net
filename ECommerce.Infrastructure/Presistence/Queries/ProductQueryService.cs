using ECommerce.Application.Products;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ECommerce.Infrastructure.Presistence.Queries;

public class ProductQueryService(ECommerceDbContext db) : IProductQueryService
{
    public async Task<IReadOnlyList<GetAllProductResponse>> GetAllProductResponse(CancellationToken ct = default)
    {
        var products = await db.Products
            .ProjectToType<GetAllProductResponse>()
            .ToListAsync(ct);

        return products;
    }

    public async Task<GetByIdProductResponse?> GetByIdProductResponse(Guid id, CancellationToken ct = default)
    {
        var product = await db.Products
            .Where(p => p.Id == id)
            .ProjectToType<GetByIdProductResponse>()
            .FirstOrDefaultAsync(ct);

        return product;



    }
}
