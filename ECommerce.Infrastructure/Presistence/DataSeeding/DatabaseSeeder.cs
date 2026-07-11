
using ECommerce.Infrastructure.Data.DbContexts;

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public class DatabaseSeeder(IEnumerable<IDataSeeder> dataSeeders , ECommerceDbContext db)
{

    public async Task SeedAll(CancellationToken ct = default) { 
    
    
        foreach(var seeder in dataSeeders)
        {
            await seeder.SeedAsync(ct);
            await db.SaveChangesAsync(ct);

        }
    }
}
