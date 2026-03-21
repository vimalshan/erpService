using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Aggregates;

namespace OrderService.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(o => o.OrderId);
        builder.Property(o => o.OrderId).HasColumnName("order_id");
        builder.Property(o => o.OrderNumber).HasColumnName("order_number").HasMaxLength(50).IsRequired();
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.Property(o => o.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.HasIndex(o => o.CustomerId);
        builder.Property(o => o.OrderDate).HasColumnName("order_date").IsRequired();
        builder.Property(o => o.RequiredDate).HasColumnName("required_date");
        builder.Property(o => o.ShippedDate).HasColumnName("shipped_date");
        builder.Property(o => o.Status).HasColumnName("status").HasMaxLength(20)
            .HasConversion<string>().IsRequired();
        builder.Property(o => o.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(18,2)");
        builder.Property(o => o.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(o => o.CreatedDate).HasColumnName("created_date").IsRequired();
        builder.Property(o => o.ModifiedDate).HasColumnName("modified_date").IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Items).AutoInclude();

        builder.Ignore(o => o.Id);
        builder.Ignore(o => o.DomainEvents);
    }
}
