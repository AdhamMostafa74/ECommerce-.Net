namespace ECommerce.Domain.Entities.Orders;

public class Payment : BaseEntity
{
    public Guid OrderId { get; private set; }

    public string Provider { get; private set; } = string.Empty;

    public string ProviderPaymentId { get; private set; } = string.Empty;
    public string? ProviderTransactionId { get; private set; }

    public PaymentStatus Status { get; private set; }

    public string ProviderOrderId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = string.Empty;

    private Payment()
    {
    }

    private Payment(
        Guid orderId,
        string provider,
        string providerPaymentId,
        string providerOrderId,
        decimal amount,
        string currency)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException(
                "Order ID is required.",
                nameof(orderId));

        if (string.IsNullOrWhiteSpace(providerOrderId))
            throw new ArgumentException(
                "Provider order ID is required.",
                nameof(providerOrderId));

        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException(
                "Payment provider is required.",
                nameof(provider));

        if (string.IsNullOrWhiteSpace(providerPaymentId))
            throw new ArgumentException(
                "Provider payment ID is required.",
                nameof(providerPaymentId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException(
                "Currency is required.",
                nameof(currency));

        OrderId = orderId;
        Provider = provider.Trim();
        ProviderPaymentId = providerPaymentId.Trim();
        Amount = amount;
        Currency = currency.Trim();
        Status = PaymentStatus.Pending;
        ProviderOrderId = providerOrderId.Trim();
    }

    public static Payment Create(
        Guid orderId,
        string provider,
        string providerOrderId,
        string providerPaymentId,
        decimal amount,
        string currency)
        => new(
            orderId,
            provider,
            providerPaymentId,
            providerOrderId,
            amount,
            currency);

    public void MarkAsPaid()
    {
        if (Status == PaymentStatus.Paid)
            return;

        if (Status == PaymentStatus.Refunded)
            throw new InvalidOperationException(
                "A refunded payment cannot be marked as paid.");

        Status = PaymentStatus.Paid;
    }


    public void SetProviderTransactionId(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException(
                "Provider transaction ID is required.",
                nameof(transactionId));

        if (ProviderTransactionId is not null &&
            ProviderTransactionId != transactionId &&
            Status != PaymentStatus.Failed)
        {
            throw new InvalidOperationException(
                "A different provider transaction ID is already recorded.");
        }

        ProviderTransactionId = transactionId.Trim();
    }
    public void MarkAsFailed()
    {
        if (Status == PaymentStatus.Paid)
            throw new InvalidOperationException(
                "A paid payment cannot be marked as failed.");

        if (Status == PaymentStatus.Refunded)
            throw new InvalidOperationException(
                "A refunded payment cannot be marked as failed.");

        Status = PaymentStatus.Failed;
    }

    public void MarkAsRefunded()
    {
        if (Status != PaymentStatus.Paid)
            throw new InvalidOperationException(
                "Only paid payments can be refunded.");

        Status = PaymentStatus.Refunded;
    }
}