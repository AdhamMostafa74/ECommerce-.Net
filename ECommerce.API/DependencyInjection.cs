using ECommerce.API.Middlewares;
using System.Text.Json.Serialization;

namespace ECommerce.API;

public static  class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services) {

        services.AddControllers();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }
}
