
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ECommerce.Application.Common.Payments;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Infrastructure.Presistence.Services.Paymob.Models;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public sealed class PaymobPaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly PaymobSettings _settings;

    public PaymobPaymentService(
        HttpClient httpClient,
        IOptions<PaymobSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<PaymentInitiationResult> CreatePaymentAsync(
        Order order,
        string customerEmail,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException(
                "Customer email is required.",
                nameof(customerEmail));

        if (order.Items.Count == 0)
            throw new InvalidOperationException(
                "Cannot initiate payment for an empty order.");

        var address = order.ShippingAddress;

        var items = order.Items.Select(item =>
        {
            var unitAmount = ToMinorUnits(item.UnitPrice);

            return new PaymobItemRequest
            {
                Name = item.ProductName,
                Amount = unitAmount,
                Description = item.ProductName,
                Quantity = item.Quantity
            };
        }).ToArray();

        var totalAmount = ToMinorUnits(order.TotalPrice);

        var calculatedAmount = items.Sum(item =>
            checked(item.Amount * item.Quantity));

        if (calculatedAmount != totalAmount)
            throw new InvalidOperationException(
                "Order total does not match the sum of its items.");

        var request = new CreateIntentionRequest
        {
            Amount = totalAmount,
            Currency = _settings.Currency,
            PaymentMethods = _settings.PaymentMethodIds,
            Items = items,

            BillingData = new PaymobBillingDataRequest
            {
                FirstName = RequiredValue(address.FirstName, "Customer"),
                LastName = RequiredValue(address.LastName, "Customer"),
                Email = customerEmail.Trim(),
                PhoneNumber = RequiredValue(
                    address.PhoneNumber, "NA"),

                Street = RequiredValue(address.Street, "NA"),
                Building = "NA",
                Apartment = "NA",
                Floor = "NA",
                City = RequiredValue(address.City, "NA"),
                State = RequiredValue(address.State, "NA"),
                Country = RequiredValue(address.Country, "NA"),
                PostalCode = RequiredValue(address.PostalCode, "NA"),
                ShippingMethod = "NA"
            },

            // Allows the callback to be correlated with this order.
            SpecialReference = order.Id.ToString("N"),

            NotificationUrl = _settings.NotificationUrl,
            RedirectionUrl = _settings.RedirectionUrl
        };

        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            "v1/intention/");

        message.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Token",
                _settings.SecretKey);

        message.Content = JsonContent.Create(request);

        using var response = await _httpClient.SendAsync(
            message,
            HttpCompletionOption.ResponseHeadersRead,
            ct);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Paymob intention creation failed with HTTP status " +
                $"{(int)response.StatusCode} ({response.StatusCode}).");
        }

        var intention = await response.Content
            .ReadFromJsonAsync<PaymobIntentionResponse>(
                cancellationToken: ct);

        if (intention is null ||
            string.IsNullOrWhiteSpace(intention.Id) ||
            string.IsNullOrWhiteSpace(intention.ClientSecret))
        {
            throw new InvalidOperationException(
                "Paymob returned an incomplete payment intention response.");
        }

        return new PaymentInitiationResult(
      intention.Id,
      intention.ClientSecret,
      _settings.Currency);
    }

    private static int ToMinorUnits(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException(
                "Payment amounts must be greater than zero.");

        if (amount != decimal.Round(amount, 2))
            throw new InvalidOperationException(
                "Payment amounts cannot contain fractions smaller than one piastre.");

        return checked(decimal.ToInt32(amount * 100m));
    }

    private static string RequiredValue(
        string? value,
        string fallback)
    {
        return string.IsNullOrWhiteSpace(value)
            ? fallback
            : value.Trim();
    }
}
