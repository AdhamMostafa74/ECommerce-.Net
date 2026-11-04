
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob.Models;

public sealed class PaymobTransactionCallback
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("obj")]
    public JsonElement Transaction { get; init; }
}
