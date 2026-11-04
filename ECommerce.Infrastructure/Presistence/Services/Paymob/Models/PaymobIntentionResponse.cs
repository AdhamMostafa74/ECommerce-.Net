using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob.Models;

public sealed class PaymobIntentionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("intention_order_id")]
    public long IntentionOrderId { get; init; }

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; init; } = string.Empty;
}