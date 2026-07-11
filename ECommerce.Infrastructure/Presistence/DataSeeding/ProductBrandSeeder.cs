using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data.Models;

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public class ProductBrandSeeder(ECommerceDbContext db) : IDataSeeder
{
    public int Order => 1;

    public  async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<ProductBrand, ProductBrandSeedModel>
            (

            db.ProductBrands,
            "Brands.json",
            b => ProductBrand.Create(b.Id, b.Name),
            ct
            );
    }
}
