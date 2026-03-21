using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesOrderService.Domain.Entities;
using SalesOrderService.Domain.Enums;
using SalesOrderService.Domain.ValueObjects;

namespace SalesOrderService.Infrastructure.Persistence.Configurations;

public sealed class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrder");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasColumnName("so_id")
               .UseIdentityColumn();

        builder.Property(x => x.SoNumber)
               .HasColumnName("so_number")
               .HasMaxLength(50)
               .IsRequired();

        builder.HasIndex(x => x.SoNumber)
               .IsUnique()
               .HasDatabaseName("IX_SalesOrder_SONumber");

        builder.Property(x => x.CustomerId)
               .HasColumnName("customer_id")
               .IsRequired();

        builder.HasIndex(x => x.CustomerId)
               .HasDatabaseName("IX_SalesOrder_Customer");

        builder.Property(x => x.WarehouseId)
               .HasColumnName("warehouse_id")
               .IsRequired();

        builder.Property(x => x.OrderDate)
               .HasColumnName("order_date")
               .IsRequired();

        builder.Property(x => x.RequestedDate)
               .HasColumnName("requested_date");

        builder.Property(x => x.Status)
               .HasColumnName("status")
               .HasMaxLength(30)
               .IsRequired()
               .HasConversion(
                   v => v.ToString().ToUpperInvariant(),
                   v => Enum.Parse<SalesOrderStatus>(v, true));

        // Money value object stored as two columns
        builder.OwnsOne(x => x.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("total_amount")
                 .HasColumnType("decimal(18,2)");
            money.Ignore(m => m.Currency); // currency stored per-line if needed
        });

        builder.Property(x => x.Notes)
               .HasColumnName("notes");

        builder.Property(x => x.CreatedBy)
               .HasColumnName("created_by")
               .HasMaxLength(50);

        builder.Property(x => x.CreatedDate)
               .HasColumnName("created_date")
               .IsRequired();

        builder.Property(x => x.ModifiedDate)
               .HasColumnName("modified_date")
               .IsRequired();

        // Aggregate-owned collection navigation
        builder.HasMany(x => x.Lines)
               .WithOne()
               .HasForeignKey(l => l.SoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}
