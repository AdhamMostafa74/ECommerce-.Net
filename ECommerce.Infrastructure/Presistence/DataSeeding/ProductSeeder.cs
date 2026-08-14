using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data.Models;

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public class ProductSeeder(ECommerceDbContext db) : IDataSeeder
{
    public int Order => 3;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<Product, ProductSeedModel>(
     db.Products,
     "Products.json",
     p => Product.Create(
         p.Name,
         p.Description,
         p.PictureUrl,
         p.Price,
         p.BrandId,
         p.ProductTypeId),
     ct);
    }
}