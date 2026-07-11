using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data.Models;
using ECommerce.Infrastructure.Presistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<Interceptor>();




        return services;
    }
}