using ECommerce.Application.Brands;
using ECommerce.Application.Products;
using ECommerce.Application.Types;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Data.DbContexts;
using ECommerce.Infrastructure.Presistence.DataSeeding;
using ECommerce.Infrastructure.Presistence.DataSeeding.Data;
using ECommerce.Infrastructure.Presistence.Interceptors;
using ECommerce.Infrastructure.Presistence.Queries;
using ECommerce.Infrastructure.Presistence.Repositories;
using ECommerce.Infrastructure.Services.Cloudinary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ECommerce.Infrastructure.Presistence.Services.CloudinaryServices;
using ECommerce.Application.Common.Cloudinary;

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

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IBrandQueryService, BrandQueryService>();
        services.AddScoped<ITypeQueryService, TypeQueryService>();


        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<Interceptor>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();


        // cloudinary


        services.Configure<CloudinarySettings>(
          configuration.GetSection(CloudinarySettings.SectionName));
        services.AddScoped<IImageService, CloudinaryImageService>();

        //redis

        var redisOptions = new ConfigurationOptions
        {
            User = configuration["Redis:User"],
            Password = configuration["Redis:Password"]
        };

        redisOptions.EndPoints.Add(
            configuration["Redis:Host"]!,
            int.Parse(configuration["Redis:Port"]!));


        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisOptions));

        services.AddScoped<IBasketRepository, RedisBasketRepository>();

        return services;
    }
}