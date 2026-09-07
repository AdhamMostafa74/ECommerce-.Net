namespace ECommerce.Domain.Entities.Orders;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public string PictureUrl { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal TotalPrice => UnitPrice * Quantity;

    private OrderItem() { }

    internal OrderItem(
        Guid orderId,
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

}