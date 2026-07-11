

using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data.Models;

namespace ECommerce.Infrastructure.Presistence.DataSeeding.Data;

public class ProductTypeSeeder(ECommerceDbContext db) : IDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await JsonSeeder.SeedIfEmpty<ProductType, ProductTypeSeedModel>
            (

            db.ProductTypes,
            "Types.json",
            b => ProductType.Create(b.Id, b.Name),
            ct
            );
    }
}
