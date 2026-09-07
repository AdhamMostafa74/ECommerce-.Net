using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = [];

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public int TotalQuantity { get; private set; }

    public Address ShippingAddress { get; private set; } = null!;

    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    private Order()
    {
    }

    private Order(
        Guid customerId,
        Address shippingAddress)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException(
                "Customer ID is required.",
                nameof(customerId));

        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
    }

    public static Order Create(
        Guid customerId,
        Address shippingAddress)
    {
        return new Order(
            customerId,
            shippingAddress);
    }

    public void AddItem(
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException(
                "Product ID is required.",
                nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException(
                "Product name is required.",
                nameof(productName));

        if (string.IsNullOrWhiteSpace(pictureUrl))
            throw new ArgumentException(
                "Picture URL is required.",
                nameof(pictureUrl));

        if (unitPrice <= 0)
            throw new ArgumentException(
                "Unit price must be greater than zero.",
                nameof(unitPrice));

        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        var item = new OrderItem(
            Id,
            productId,
            productName,
            pictureUrl,
            unitPrice,
            quantity);

        _items.Add(item);

        RecalculateTotals();
    }

    private void RecalculateTotals()
    {
        TotalPrice = _items.Sum(x => x.TotalPrice);
        TotalQuantity = _items.Sum(x => x.Quantity);
    }
}