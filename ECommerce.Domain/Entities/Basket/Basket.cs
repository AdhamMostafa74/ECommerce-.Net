namespace ECommerce.Domain.Entities.Basket
{
    public class Basket
    {
        private readonly List<BasketItem> _items = [];

        public Guid Id { get; private set; }

        public IReadOnlyCollection<BasketItem> Items =>
            _items.AsReadOnly();

        private Basket()
        {
        }

        public Basket(Guid id)
        {
            Id = id;
        }

        public void AddItem(BasketItem item)
        {
            var existingItem = _items.FirstOrDefault(
                x => x.ProductId == item.ProductId);

            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(item.Quantity);
                return;
            }

            _items.Add(item);
        }

        public bool UpdateItemQuantity(
            Guid productId,
            int quantity)
        {
            var item = _items.FirstOrDefault(
                x => x.ProductId == productId);

            if (item is null)
                return false;

            item.SetQuantity(quantity);

            return true;
        }

        public bool RemoveItem(Guid productId)
        {
            var item = _items.FirstOrDefault(
                x => x.ProductId == productId);

            if (item is null)
                return false;

            _items.Remove(item);

            return true;
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}