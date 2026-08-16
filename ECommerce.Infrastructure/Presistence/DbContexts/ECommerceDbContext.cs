using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data.DbContexts;

public class ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
    public DbSet<ProductType> ProductTypes => Set<ProductType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);
    }
}