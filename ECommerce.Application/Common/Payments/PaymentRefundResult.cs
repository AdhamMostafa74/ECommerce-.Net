namespace ECommerce.Application.Common.Payments;

public sealed record PaymentRefundResult(
    string RefundTransactionId,
    decimal Amount,
    string Currency);

