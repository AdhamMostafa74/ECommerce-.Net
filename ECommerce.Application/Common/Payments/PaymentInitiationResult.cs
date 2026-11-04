namespace ECommerce.Application.Common.Payments;

public sealed record PaymentInitiationResult(
    string PaymentIntentId,
    string ProviderOrderId,
    string ClientSecret,
    string Currency);