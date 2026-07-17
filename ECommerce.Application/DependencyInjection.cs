using ECommerce.Application.Brands.Queries;
using ECommerce.Application.Products.Queries;
using ECommerce.Application.Types.Queries;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingConfigs).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddScoped<GetAllProductsQuery>();
        services.AddScoped<GetByIdProductsQuery>();

        services.AddScoped<GetAllBrandsQuery>();
        services.AddScoped<GetByIdBrandQuery>();
        services.AddScoped<GetByIdTypeQuery>();
        services.AddScoped<GetAllTypesQuery>();

        return services;
    }
}
