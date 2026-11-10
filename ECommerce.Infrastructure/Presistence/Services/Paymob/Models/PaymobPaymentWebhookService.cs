
using System.Text.Json;
using ECommerce.Application.Common.Payments;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Infrastructure.Presistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob.Models;

public sealed class PaymobPaymentWebhookService(
    ECommerceDbContext dbContext,
    IOptions<PaymobSettings> settings,
    ILogger<PaymobPaymentWebhookService> logger)
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
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Failed to parse Paymob webhook callback JSON body.");
            return false;
        }

        using var _ = document;

        var root = document.RootElement;

        if (!PaymobHmacVerifier.VerifyTransaction(
                root,
                hmac,
                _settings.HmacSecret))
        {
            logger.LogWarning("Paymob webhook callback HMAC verification failed.");
            return false;
        }

        if (!TryGetString(root, "type", out var type) ||
            type != "TRANSACTION")
        {
            logger.LogInformation("Ignoring non-TRANSACTION Paymob callback type: {CallbackType}.", type);
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
            logger.LogWarning("Paymob transaction callback missing required fields.");
            return false;
        }

        var providerOrderIdStr = providerOrderId.ToString();

        var payment = await dbContext.Payments
            .SingleOrDefaultAsync(
                payment =>
                    payment.Provider == "Paymob" &&
                    payment.ProviderOrderId == providerOrderIdStr,
                ct);

        Order? order = null;

        if (payment is not null)
        {
            order = await dbContext.Orders
                .SingleOrDefaultAsync(
                    order => order.Id == payment.OrderId,
                    ct);
        }
        else
        {
            // Self-healing: if the payment entity was not persisted during order creation,
            // attempt correlation via special_reference which carries the Order.Id
            if (TryGetString(providerOrder, "special_reference", out var specialRef) ||
                TryGetString(transaction, "special_reference", out specialRef) ||
                TryGetString(providerOrder, "merchant_order_id", out specialRef))
            {
                if (Guid.TryParse(specialRef, out var correlatedOrderId))
                {
                    order = await dbContext.Orders
                        .Include(o => o.Payments)
                        .SingleOrDefaultAsync(o => o.Id == correlatedOrderId, ct);

                    if (order is not null)
                    {
                        payment = order.Payments.FirstOrDefault(p =>
                            p.Provider == "Paymob" &&
                            p.ProviderOrderId == providerOrderIdStr);

                        if (payment is null)
                        {
                            var providerPaymentId = TryGetString(transaction, "intention_id", out var intentionId)
                                ? intentionId
                                : providerOrderIdStr;

                            payment = Payment.Create(
                                order.Id,
                                "Paymob",
                                providerOrderIdStr,
                                providerPaymentId,
                                order.TotalPrice,
                                currency);

                            order.AddPayment(payment);
                            dbContext.Payments.Add(payment);

                            logger.LogWarning(
                                "Self-healed missing Payment record for Order {OrderId} using Paymob ProviderOrderId {ProviderOrderId}.",
                                order.Id,
                                providerOrderIdStr);
                        }
                    }
                }
            }
        }

        if (payment is null || order is null)
        {
            logger.LogWarning(
                "Webhook received for unknown payment/order. ProviderOrderId: {ProviderOrderId}, TransactionId: {TransactionId}.",
                providerOrderId,
                transactionId);
            return false;
        }

        var callbackAmount = amountCents / 100m;

        if (callbackAmount != payment.Amount ||
            !string.Equals(
                currency,
                payment.Currency,
                StringComparison.OrdinalIgnoreCase))
        {
            logger.LogWarning(
                "Payment amount/currency mismatch for Order {OrderId}. Expected: {ExpectedAmount} {ExpectedCurrency}, Received: {ReceivedAmount} {ReceivedCurrency}.",
                order.Id,
                payment.Amount,
                payment.Currency,
                callbackAmount,
                currency);
            return false;
        }

        // The callback is authentic and belongs to this payment,
        // but the transaction is not in a final state yet.
        if (pending)
            return true;

        var providerTransactionId = transactionId.ToString();

        try
        {
            if (isRefunded)
            {
                if (payment.Status == PaymentStatus.Refunded)
                    return true;

                if (payment.Status == PaymentStatus.Pending)
                {
                    payment.MarkAsPaid();
                    order.MarkPaymentAsPaid();
                }

                if (payment.Status == PaymentStatus.Paid)
                {
                    if (payment.ProviderTransactionId is null)
                        payment.SetProviderTransactionId(providerTransactionId);

                    payment.MarkAsRefunded();
                    order.MarkPaymentAsRefunded();

                    if (order.Status == OrderStatus.Pending ||
                        order.Status == OrderStatus.Processing)
                    {
                        order.Cancel();
                    }

                    await SaveChangesSafelyAsync(ct);
                }

                return true;
            }

            if (isVoided)
            {
                if (payment.Status == PaymentStatus.Failed ||
                    payment.Status == PaymentStatus.Refunded)
                {
                    return true;
                }

                if (payment.ProviderTransactionId is null)
                    payment.SetProviderTransactionId(providerTransactionId);

                if (payment.Status == PaymentStatus.Paid)
                {
                    payment.MarkAsRefunded();
                    order.MarkPaymentAsRefunded();
                }
                else if (payment.Status == PaymentStatus.Pending)
                {
                    payment.MarkAsFailed();
                    order.MarkPaymentAsFailed();
                }

                if (order.Status == OrderStatus.Pending ||
                    order.Status == OrderStatus.Processing)
                {
                    order.Cancel();
                }

                await SaveChangesSafelyAsync(ct);
                return true;
            }

            // If already marked as Paid:
            if (payment.Status == PaymentStatus.Paid)
            {
                if (payment.ProviderTransactionId == providerTransactionId)
                    return true;

                logger.LogInformation(
                    "Order {OrderId} is already Paid with transaction {ExistingTxnId}. Ignoring callback for transaction {NewTxnId}.",
                    order.Id,
                    payment.ProviderTransactionId,
                    providerTransactionId);
                return true;
            }

            // If previously marked as Failed, but another failed transaction arrives:
            if (payment.Status == PaymentStatus.Failed && !success)
            {
                return true;
            }

            payment.SetProviderTransactionId(providerTransactionId);

            if (success)
            {
                payment.MarkAsPaid();
                order.MarkPaymentAsPaid();

                if (order.Status == OrderStatus.Pending)
                {
                    order.MarkAsProcessing();
                }
            }
            else
            {
                payment.MarkAsFailed();
                order.MarkPaymentAsFailed();
            }

            await SaveChangesSafelyAsync(ct);
            return true;
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(
                ex,
                "Domain state transition conflict for Order {OrderId}, Payment {PaymentId}, Transaction {TransactionId}.",
                order.Id,
                payment.Id,
                providerTransactionId);
            return true;
        }
    }

    private async Task<bool> SaveChangesSafelyAsync(CancellationToken ct)
    {
        try
        {
            await dbContext.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning(
                ex,
                "DbUpdateException during Paymob webhook processing. Handled as idempotent duplicate callback.");
            return true;
        }
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

        if (!element.TryGetProperty(propertyName, out var property))
            return false;

        switch (property.ValueKind)
        {
            case JsonValueKind.True:
                value = true;
                return true;

            case JsonValueKind.False:
                value = false;
                return true;

            case JsonValueKind.Null:
                value = false;
                return true;

            default:
                return false;
        }
    }
}
