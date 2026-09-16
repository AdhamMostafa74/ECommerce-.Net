using System.ComponentModel.DataAnnotations;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public sealed class PaymobSettings
{
    public const string SectionName = "Paymob";

    [Required]
    public string BaseUrl { get; init; } = string.Empty;

    [Required]
    public string SecretKey { get; init; } = string.Empty;

    [Required]
    public string Currency { get; init; } = string.Empty;

    [Required]
    public string NotificationUrl { get; init; } = string.Empty;

    [Required]
    public string RedirectionUrl { get; init; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<int> PaymentMethodIds { get; init; } = [];
}