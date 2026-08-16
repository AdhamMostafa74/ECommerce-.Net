using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(p => p.PictureUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        // Brand Relationship
        builder.HasOne(p => p.ProductBrand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.ProductBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product Type Relationship
        builder.HasOne(p => p.ProductType)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}