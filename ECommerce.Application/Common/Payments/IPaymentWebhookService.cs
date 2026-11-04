namespace ECommerce.Application.Common.Payments;

public interface IPaymentWebhookService
{
    Task<bool> ProcessPaymobCallbackAsync(
        string callbackBody,
        string hmac,
        CancellationToken ct = default);
}