
using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob.Models;

public sealed class PaymobBillingDataRequest
{
    [JsonPropertyName("first_name")]
    public required string FirstName { get; init; }

    [JsonPropertyName("last_name")]
    public required string LastName { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("phone_number")]
    public required string PhoneNumber { get; init; }

    [JsonPropertyName("street")]
    public required string Street { get; init; }

    [JsonPropertyName("building")]
    public required string Building { get; init; }

    [JsonPropertyName("apartment")]
    public required string Apartment { get; init; }

    [JsonPropertyName("floor")]
    public required string Floor { get; init; }

    [JsonPropertyName("city")]
    public required string City { get; init; }

    [JsonPropertyName("state")]
    public required string State { get; init; }

    [JsonPropertyName("country")]
    public required string Country { get; init; }

    [JsonPropertyName("postal_code")]
    public required string PostalCode { get; init; }

    [JsonPropertyName("shipping_method")]
    public required string ShippingMethod { get; init; }
}
