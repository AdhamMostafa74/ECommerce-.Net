using ECommerce.Application.Brands;
using ECommerce.Application.Products;
using ECommerce.Application.Types;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data;
using ECommerce.Infrastructure.Presistence.Interceptors;
using ECommerce.Infrastructure.Presistence.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ECommerceDbContext>((sp, options) =>
              {
                  options.UseSqlServer(
                      configuration.GetConnectionString("DefaultConnection"));
                  options.AddInterceptors(
                      sp.GetRequiredService<Interceptor>()
                      );

              });




        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();

        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<Interceptor>();




        return services;
    }
}