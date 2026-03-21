using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");
        builder.HasKey(i => i.OrderItemId);
        builder.Property(i => i.OrderItemId).HasColumnName("order_item_id");
        builder.Property(i => i.OrderId).HasColumnName("order_id").IsRequired();
        builder.HasIndex(i => i.OrderId);
        builder.Property(i => i.ProductId).HasColumnName("product_id").IsRequired();
        builder.HasIndex(i => i.ProductId);
        builder.Property(i => i.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(i => i.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,4)").IsRequired();
        builder.Property(i => i.Discount).HasColumnName("discount").HasColumnType("decimal(18,2)");
        builder.Property(i => i.Notes).HasColumnName("notes");

        builder.Ignore(i => i.DomainEvents);
    }
}
