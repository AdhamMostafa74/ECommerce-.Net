using ECommerce.Domain.Entities.Basket;
using ECommerce.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace ECommerce.Infrastructure.Presistence.Repositories
{
    public sealed class RedisBasketRepository(
        IConnectionMultiplexer redis) : IBasketRepository


    {
        private readonly IDatabase _database = redis.GetDatabase();

        // create redis key for basket

        private static string GetKey(Guid basketId)
    => $"basket:{basketId}";


        public async Task<Basket?> GetAsync(
      Guid basketId,
      CancellationToken ct = default)
        {
            var key = GetKey(basketId);

            var basketData = await _database.StringGetAsync(key);

            if (!basketData.HasValue)
                return null;

            return JsonSerializer.Deserialize<Basket>(
                basketData.ToString());
        }

        public async Task SaveAsync(
             Basket basket,
             CancellationToken ct = default)
        {
            var key = GetKey(basket.Id);

            var basketData = JsonSerializer.Serialize(basket);

            await _database.StringSetAsync(
                key,
                basketData,
                TimeSpan.FromDays(7));

            var savedBasket = await _database.StringGetAsync(key);

        }

        public Task DeleteAsync(Guid basketId, CancellationToken ct = default)
        {
            var key = GetKey(basketId);
            return _database.KeyDeleteAsync(key);
        }
    }
}