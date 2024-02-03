using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProject.Models.Model;

namespace MyProject.Models.Configuration;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProduct");

        builder.HasOne<Order>(cp => cp.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(cp => cp.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>(cp => cp.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(cp => cp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}