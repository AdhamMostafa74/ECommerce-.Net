using ECommerce.Domain.Entities.Basket;
using ECommerce.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;
using ECommerce.Infrastructure.Presistence.Redis.Models;

namespace ECommerce.Infrastructure.Presistence.Repositories;

public sealed class RedisBasketRepository(
    IConnectionMultiplexer redis) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    // Create Redis key for user's basket
    private static string GetKey(Guid userId)
        => $"basket:{userId}";

    public async Task<BasketEntity?> GetAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var key = GetKey(userId);

        var basketData = await _database.StringGetAsync(key);

        if (!basketData.HasValue)
            return null;

        var data = JsonSerializer.Deserialize<RedisBasketModel>(
            basketData.ToString());

        if (data is null)
            return null;

        var basket = new BasketEntity(data.Id);

        foreach (var item in data.Items)
        {
            basket.AddItem(
                new BasketItem(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.Price,
                    item.Quantity));
        }

        return basket;
    }

    public async Task SaveAsync(
        Guid userId,
        BasketEntity basket,
        CancellationToken ct = default)
    {
        var key = GetKey(userId);

        var basketData = JsonSerializer.Serialize(basket);

        await _database.StringSetAsync(
            key,
            basketData,
            TimeSpan.FromDays(7));
    }

    public Task DeleteAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var key = GetKey(userId);

        return _database.KeyDeleteAsync(key);
    }
}