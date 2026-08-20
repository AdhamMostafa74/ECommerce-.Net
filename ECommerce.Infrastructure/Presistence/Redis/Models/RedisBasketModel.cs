namespace ECommerce.Infrastructure.Presistence.Redis.Models;

public sealed class RedisBasketModel
{
    public Guid Id { get; set; }

    public List<RedisBasketItemModel> Items { get; set; } = [];
}
