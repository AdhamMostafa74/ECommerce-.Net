namespace ECommerce.Infrastructure.Presistence.Redis.Models
{
    public sealed class RedisBasketItemModel
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}