namespace ECommerce.Domain.Entities;

public class BasketItem
{
    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public string PictureUrl { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int Quantity { get; private set; }

    private BasketItem()
    {
    }

    public BasketItem(
        Guid productId,
        string productName,
        string pictureUrl,
        decimal price,
        int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
        Price = price;
        Quantity = quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}