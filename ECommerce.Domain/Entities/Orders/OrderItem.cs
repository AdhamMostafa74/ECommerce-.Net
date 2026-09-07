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

    public decimal TotalPrice =>
        UnitPrice * Quantity;

    private OrderItem()
    {
    }

    internal OrderItem(
        Guid orderId,
        Guid productId,
        string productName,
        string pictureUrl,
        decimal unitPrice,
        int quantity)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException(
                "Order ID is required.",
                nameof(orderId));

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

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName.Trim();
        PictureUrl = pictureUrl.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}