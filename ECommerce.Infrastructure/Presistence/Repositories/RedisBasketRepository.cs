using System.Text.Json;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using StackExchange.Redis;

namespace ECommerce.Infrastructure.Presistence.Repositories;

public sealed class RedisBasketRepository(
    IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    private static string GetKey(Guid basketId)
        => $"basket:{basketId}";

    public async Task<Basket?> GetAsync(
        Guid basketId,
        CancellationToken ct = default)
    {
        var key = GetKey(basketId);

        var data = await _database.StringGetAsync(key);

        if (data.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<Basket>(
            data.ToString());
    }

    public async Task SaveAsync(
        Basket basket,
        CancellationToken ct = default)
    {
        var key = GetKey(basket.Id);

        var data = JsonSerializer.Serialize(basket);

        await _database.StringSetAsync(
            key,
            data,
            TimeSpan.FromDays(30));
    }

    public async Task DeleteAsync(
        Guid basketId,
        CancellationToken ct = default)
    {
        var key = GetKey(basketId);

        await _database.KeyDeleteAsync(key);
    }
}