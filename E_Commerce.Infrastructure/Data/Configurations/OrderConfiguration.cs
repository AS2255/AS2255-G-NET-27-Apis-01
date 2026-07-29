using E_Commerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("order");

            builder.OwnsOne(O => O.ShipToAddress, address =>
            {
                address.Property(A => A.FirstName).HasColumnName("FirstName").HasMaxLength(50);
                address.Property(A => A.LastName).HasColumnName("LastName").HasMaxLength(50);
                address.Property(A => A.Street).HasColumnName("Street").HasMaxLength(50);
                address.Property(A => A.City).HasColumnName("City").HasMaxLength(50);
                address.Property(A => A.Country).HasColumnName("Country").HasMaxLength(50);
            });

            builder.HasMany(O => O.OrderItems)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(O => O.SubTotal).HasColumnType("decimal(8,2)");

            builder.Property(O => O.Status).HasConversion<string>().HasMaxLength(50);

            builder.HasOne(O => O.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(O => O.DeliveryMethodId); 
        }
    }
}
