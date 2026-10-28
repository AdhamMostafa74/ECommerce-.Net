
using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob.Models;

public sealed class CreateIntentionRequest
{
    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("payment_methods")]
    public required IReadOnlyCollection<int> PaymentMethods { get; init; }

    [JsonPropertyName("items")]
    public required IReadOnlyCollection<PaymobItemRequest> Items { get; init; }

    [JsonPropertyName("billing_data")]
    public required PaymobBillingDataRequest BillingData { get; init; }

    [JsonPropertyName("special_reference")]
    public required string SpecialReference { get; init; }

    [JsonPropertyName("notification_url")]
    public required string NotificationUrl { get; init; }

    [JsonPropertyName("redirection_url")]
    public required string RedirectionUrl { get; init; }
}

public sealed class PaymobItemRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("amount")]
    public int Amount { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}
