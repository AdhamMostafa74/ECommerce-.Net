using ECommerce.Application.Common.Behaviors;
using ECommerce.Application.Common.Mapping;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Products.Commands.CreateProduct;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerce.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingConfiguration).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(
     typeof(CreateProductValidator).Assembly);

        services.AddTransient<IResultFactory, ResultFactory>();

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        return services;
    }
}
