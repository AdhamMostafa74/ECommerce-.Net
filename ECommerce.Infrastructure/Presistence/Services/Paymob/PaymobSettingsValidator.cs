using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public sealed class PaymobSettingsValidator
    : IValidateOptions<PaymobSettings>
{
    public ValidateOptionsResult Validate(
        string? name,
        PaymobSettings options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
            errors.Add("Paymob BaseUrl is required.");

        if (string.IsNullOrWhiteSpace(options.SecretKey))
            errors.Add("Paymob SecretKey is required.");

        if (string.IsNullOrWhiteSpace(options.Currency))
            errors.Add("Paymob Currency is required.");

        if (options.PaymentMethodIds is null ||
            options.PaymentMethodIds.Count == 0)
        {
            errors.Add(
                "At least one Paymob PaymentMethodId is required.");
        }

        if (!Uri.TryCreate(
                options.BaseUrl,
                UriKind.Absolute,
                out var baseUri) ||
            baseUri.Scheme != Uri.UriSchemeHttps)
        {
            errors.Add(
                "Paymob BaseUrl must be a valid HTTPS URL.");
        }

        if (errors.Count > 0)
            return ValidateOptionsResult.Fail(errors);

        return ValidateOptionsResult.Success;
    }
}