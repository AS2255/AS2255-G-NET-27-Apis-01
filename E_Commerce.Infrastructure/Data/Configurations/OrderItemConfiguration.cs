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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.Property(OI => OI.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(OI => OI.Product, product =>
            {
                product.Property(P => P.ProductName).HasColumnName("ProductName").HasMaxLength(100);
                product.Property(P => P.PictureUrl).HasColumnName("PictureUrl").HasMaxLength(200);
            });
            
            builder.OwnsOne(OI => OI.Product);
        }
    }
}
