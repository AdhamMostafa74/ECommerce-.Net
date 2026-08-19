using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ECommerce.Infrastructure.Presistence.DataSeeding;

public class JsonSeeder
{
    private static readonly JsonSerializerOptions options = new()
    {

        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedIfEmpty<TEntity, TModel>(
        DbSet<TEntity> dbSet,
        string fileName,
        Func<TModel, TEntity> map,
        CancellationToken ct = default
        ) where TEntity : BaseEntity
    {

        if (await dbSet.AnyAsync(ct)) return;
        var filePath = Path.Combine(AppContext.BaseDirectory , "Presistence", "DataSeeding","Data" , fileName);
        Console.WriteLine(filePath);
        if (!File.Exists(filePath)) return;

        await using var stream = File.OpenRead(filePath);

        var models = await JsonSerializer.DeserializeAsync<List<TModel>>(stream, options, ct);

        if(models == null || !models.Any()) return;

        var entities = models.Select(map);

        await dbSet.AddRangeAsync(entities,ct);
    }
}
