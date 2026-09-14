namespace ECommerce.Domain.Entities.Orders
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4
    }

    public enum PaymentMethod
    {
        CashOnDelivery = 1,
        Online = 2
    }
}