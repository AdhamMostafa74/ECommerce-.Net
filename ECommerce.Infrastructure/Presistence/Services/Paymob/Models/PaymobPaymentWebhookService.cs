
using System.Text.Json;
using ECommerce.Application.Common.Payments;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Infrastructure.Presistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public sealed class PaymobPaymentWebhookService(
    ECommerceDbContext dbContext,
    IOptions<PaymobSettings> settings)
    : IPaymentWebhookService
{
    private readonly PaymobSettings _settings = settings.Value;

    public async Task<bool> ProcessPaymobCallbackAsync(
        string callbackBody,
        string hmac,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(callbackBody) ||
            string.IsNullOrWhiteSpace(hmac))
        {
            return false;
        }

        JsonDocument document;

        try
        {
            document = JsonDocument.Parse(callbackBody);
        }
        catch (JsonException)
        {
            return false;
        }

        using var _ = document;

        var root = document.RootElement;

        if (!PaymobHmacVerifier.VerifyTransaction(
                root,
                hmac,
                _settings.HmacSecret))
        {
            return false;
        }

        if (!TryGetString(root, "type", out var type) ||
            type != "TRANSACTION")
        {
            return false;
        }

        if (!root.TryGetProperty("obj", out var transaction))
            return false;

        if (!TryGetInt64(transaction, "id", out var transactionId) ||
            !transaction.TryGetProperty("order", out var providerOrder) ||
            !TryGetInt64(providerOrder, "id", out var providerOrderId) ||
            !TryGetInt64(transaction, "amount_cents", out var amountCents) ||
            !TryGetString(transaction, "currency", out var currency) ||
            !TryGetBoolean(transaction, "success", out var success) ||
            !TryGetBoolean(transaction, "pending", out var pending) ||
            !TryGetBoolean(transaction, "is_refunded", out var isRefunded) ||
            !TryGetBoolean(transaction, "is_voided", out var isVoided))
        {
            return false;
        }

        var payment = await dbContext.Payments
            .SingleOrDefaultAsync(
                payment =>
                    payment.Provider == "Paymob" &&
                    payment.ProviderOrderId == providerOrderId.ToString(),
                ct);

        if (payment is null)
            return false;

        var order = await dbContext.Orders
            .SingleOrDefaultAsync(
                order => order.Id == payment.OrderId,
                ct);

        if (order is null)
            return false;

        var callbackAmount = amountCents / 100m;

        if (callbackAmount != payment.Amount ||
            !string.Equals(
                currency,
                payment.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // The callback is authentic and belongs to this payment,
        // but the transaction is not in a final state yet.
        if (pending)
            return true;

        // Refund and void processing will be implemented separately.
        // Accept the callback so the provider does not needlessly retry it.
        if (isRefunded || isVoided)
            return true;

        var providerTransactionId = transactionId.ToString();

        // Make repeated callbacks for the same final transaction idempotent.
        if (payment.Status == PaymentStatus.Paid)
        {
            return payment.ProviderTransactionId == providerTransactionId;
        }

        if (payment.Status == PaymentStatus.Failed)
        {
            return payment.ProviderTransactionId == providerTransactionId;
        }

        payment.SetProviderTransactionId(providerTransactionId);

        if (success)
        {
            payment.MarkAsPaid();
            order.MarkPaymentAsPaid();
        }
        else
        {
            payment.MarkAsFailed();
            order.MarkPaymentAsFailed();
        }

        await dbContext.SaveChangesAsync(ct);

        return true;
    }

    private static bool TryGetInt64(
        JsonElement element,
        string propertyName,
        out long value)
    {
        value = 0;

        return element.TryGetProperty(propertyName, out var property) &&
               property.TryGetInt64(out value);
    }

    private static bool TryGetString(
        JsonElement element,
        string propertyName,
        out string value)
    {
        value = string.Empty;

        if (!element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = property.GetString() ?? string.Empty;

        return !string.IsNullOrWhiteSpace(value);
    }

    private static bool TryGetBoolean(
        JsonElement element,
        string propertyName,
        out bool value)
    {
        value = false;

        return element.TryGetProperty(propertyName, out var property) &&
               (property.ValueKind == JsonValueKind.True ||
                property.ValueKind == JsonValueKind.False) &&
               property.GetBoolean();
    }
}
