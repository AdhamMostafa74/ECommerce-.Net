using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using ECommerce.API.Middlewares;
using ECommerce.API.Responses;
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode =
                    StatusCodes.Status429TooManyRequests;

                var response = ApiResponse<object>.ApiFailure(
                    [new Error(
                        "RateLimit.Exceeded",
                        "Too many requests. Please try again later.",
                        ErrorType.Failure)],
                    context.HttpContext);

                await context.HttpContext.Response.WriteAsJsonAsync(
                    response,
                    cancellationToken);
            };

            options.AddPolicy("SensitiveAnonymous", httpContext =>
            {
                var partitionKey =
                    httpContext.Connection.RemoteIpAddress?.ToString()
                    ?? "anonymous";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }
}
