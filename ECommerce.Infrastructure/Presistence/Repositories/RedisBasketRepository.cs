using ECommerce.Domain.Entities.Basket;
using ECommerce.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;
using ECommerce.Infrastructure.Presistence.Redis.Models;

namespace ECommerce.Infrastructure.Presistence.Repositories
{
    public sealed class RedisBasketRepository(
        IConnectionMultiplexer redis) : IBasketRepository


    {
        private readonly IDatabase _database = redis.GetDatabase();

        // create redis key for basket

        private static string GetKey(Guid basketId)
    => $"basket:{basketId}";


        public async Task<BasketEntity?> GetAsync(
      Guid basketId,
      CancellationToken ct = default)
        {
            var key = GetKey(basketId);


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
             BasketEntity basket,
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