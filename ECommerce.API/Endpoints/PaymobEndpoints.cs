using ECommerce.Application.Common.Payments;

namespace ECommerce.API.Endpoints;

public static class PaymobEndpoints
{
    public static IEndpointRouteBuilder MapPaymobEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/v1/payments/paymob/callback",
            async (
                HttpRequest request,
                IPaymentWebhookService webhookService,
                CancellationToken ct) =>
            {
                var hmac = request.Query["hmac"].ToString();

                if (string.IsNullOrWhiteSpace(hmac))
                    return Results.BadRequest();

                using var reader = new StreamReader(request.Body);

                var callbackBody = await reader.ReadToEndAsync(ct);

                var processed = await webhookService
                    .ProcessPaymobCallbackAsync(
                        callbackBody,
                        hmac,
                        ct);

                return processed
                    ? Results.Ok()
                    : Results.BadRequest();
            })
            .AllowAnonymous()
            .WithTags("Payments")
            .WithName("PaymobCallback")
            .WithSummary("Receive Paymob transaction callbacks");

        return endpoints;
    }
}