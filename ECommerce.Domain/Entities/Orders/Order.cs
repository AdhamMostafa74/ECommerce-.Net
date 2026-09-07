namespace ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = [];

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public int TotalQuantity { get; private set; }

    public Address ShippingAddress { get; private set; } = null!;

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    private Order(
        Guid customerId,
        Address shippingAddress)
    {
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
    }

    public static Order Create(
        Guid customerId,
        Address shippingAddress)
    {
        return new Order(customerId, shippingAddress);
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);

        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        TotalPrice = _items.Sum(x => x.TotalPrice);
        TotalQuantity = _items.Sum(x => x.Quantity);
    }
}