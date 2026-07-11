

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public interface IDataSeeder
{
    public int Order { get; }

    public Task SeedAsync(CancellationToken ct = default);
}
