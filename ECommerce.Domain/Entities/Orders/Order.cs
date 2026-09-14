using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Entities.Orders;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = [];

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

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
        Address shippingAddress,
        PaymentMethod paymentMethod)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException(
                "Customer ID is required.",
                nameof(customerId));

        CustomerId = customerId;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
        PaymentStatus = PaymentStatus.Pending;
        PaymentMethod = paymentMethod;
    }

    public static Order Create(
        Guid customerId,
        Address shippingAddress,
        PaymentMethod paymentMethod)
    {
        return new Order(
            customerId,
            shippingAddress,
            paymentMethod);
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

    public void MarkAsProcessing()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Only pending orders can be processed.");

        if (PaymentMethod == PaymentMethod.Online &&
            PaymentStatus != PaymentStatus.Paid)
        {
            throw new InvalidOperationException(
                "An online order must be paid before processing.");
        }

        Status = OrderStatus.Processing;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Processing)
            throw new InvalidOperationException(
                "Only processing orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException(
                "Only shipped orders can be delivered.");

        if (PaymentMethod == PaymentMethod.CashOnDelivery)
            PaymentStatus = PaymentStatus.Paid;

        Status = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.Pending &&
            Status != OrderStatus.Processing)
        {
            throw new InvalidOperationException(
                "Only pending or processing orders can be cancelled.");
        }

        Status = OrderStatus.Cancelled;
    }

    public void MarkPaymentAsPaid()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            return;

        if (PaymentStatus == PaymentStatus.Refunded)
            throw new InvalidOperationException(
                "A refunded payment cannot be marked as paid.");

        PaymentStatus = PaymentStatus.Paid;
    }

    public void MarkPaymentAsFailed()
    {
        if (PaymentStatus == PaymentStatus.Paid)
            throw new InvalidOperationException(
                "A paid payment cannot be marked as failed.");

        if (PaymentStatus == PaymentStatus.Refunded)
            throw new InvalidOperationException(
                "A refunded payment cannot be marked as failed.");

        PaymentStatus = PaymentStatus.Failed;
    }

    public void MarkPaymentAsRefunded()
    {
        if (PaymentStatus != PaymentStatus.Paid)
            throw new InvalidOperationException(
                "Only paid payments can be refunded.");

        PaymentStatus = PaymentStatus.Refunded;
    }
    private void RecalculateTotals()
    {
        TotalPrice = _items.Sum(x => x.TotalPrice);
        TotalQuantity = _items.Sum(x => x.Quantity);
    }
}