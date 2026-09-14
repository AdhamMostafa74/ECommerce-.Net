using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Presistence.Configs;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CustomerId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(x => x.PaymentStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalQuantity)
            .IsRequired();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.OwnsOne(x => x.ShippingAddress, address =>
        {
            address.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.PhoneNumber)
                .HasMaxLength(30)
                .IsRequired();

            address.Property(x => x.Street)
                .HasMaxLength(250)
                .IsRequired();

            address.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.State)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(x => x.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            address.Property(x => x.Country)
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}