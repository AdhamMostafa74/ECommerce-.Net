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

        if (string.IsNullOrWhiteSpace(options.SecretKey))
            errors.Add("Paymob SecretKey is required.");

        if (string.IsNullOrWhiteSpace(options.PublicKey))
            errors.Add("Paymob PublicKey is required.");

        if (string.IsNullOrWhiteSpace(options.HmacSecret))
            errors.Add("Paymob HmacSecret is required.");

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            errors.Add("Paymob BaseUrl is required.");
        }
        else if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var baseUri) ||
                 baseUri.Scheme != Uri.UriSchemeHttps)
        {
            errors.Add("Paymob BaseUrl must be a valid HTTPS URL.");
        }

        if (string.IsNullOrWhiteSpace(options.NotificationUrl))
        {
            errors.Add("Paymob NotificationUrl is required.");
        }
        else if (!Uri.TryCreate(options.NotificationUrl, UriKind.Absolute, out var notifUri) ||
                 notifUri.Scheme != Uri.UriSchemeHttps)
        {
            errors.Add("Paymob NotificationUrl must be a valid HTTPS URL.");
        }

        if (string.IsNullOrWhiteSpace(options.RedirectionUrl))
        {
            errors.Add("Paymob RedirectionUrl is required.");
        }
        else if (!Uri.TryCreate(options.RedirectionUrl, UriKind.Absolute, out var redirUri) ||
                 redirUri.Scheme != Uri.UriSchemeHttps)
        {
            errors.Add("Paymob RedirectionUrl must be a valid HTTPS URL.");
        }

        if (string.IsNullOrWhiteSpace(options.Currency))
        {
            errors.Add("Paymob Currency is required.");
        }
        else if (options.Currency.Trim().Length != 3 || !options.Currency.All(char.IsLetter))
        {
            errors.Add("Paymob Currency must be a valid 3-letter ISO currency code (e.g. EGP, USD).");
        }

        if (options.PaymentMethodIds is null || options.PaymentMethodIds.Count == 0)
        {
            errors.Add("At least one Paymob PaymentMethodId is required.");
        }
        else if (options.PaymentMethodIds.Any(id => id <= 0))
        {
            errors.Add("All Paymob PaymentMethodIds must be positive integers.");
        }

        if (errors.Count > 0)
            return ValidateOptionsResult.Fail(errors);

        return ValidateOptionsResult.Success;
    }
}
