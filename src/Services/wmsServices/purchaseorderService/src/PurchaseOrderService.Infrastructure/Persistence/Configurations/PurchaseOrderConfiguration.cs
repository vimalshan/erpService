using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Domain.Enums;

namespace PurchaseOrderService.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrder");

        builder.HasKey(po => po.Id);
        builder.Property(po => po.Id).HasColumnName("po_id").UseIdentityColumn();

        builder.Property(po => po.PoNumber).HasColumnName("po_number").HasMaxLength(50).IsRequired();
        builder.HasIndex(po => po.PoNumber).IsUnique();

        builder.Property(po => po.SupplierId).HasColumnName("supplier_id").IsRequired();
        builder.HasIndex(po => po.SupplierId);

        builder.Property(po => po.WarehouseId).HasColumnName("warehouse_id").IsRequired();

        builder.Property(po => po.OrderDate).HasColumnName("order_date").HasColumnType("date").IsRequired();
        builder.Property(po => po.ExpectedDate).HasColumnName("expected_date").HasColumnType("date");

        builder.Property(po => po.Status).HasColumnName("status").HasMaxLength(30).IsRequired()
            .HasConversion(
                v => v.ToDbString(),
                v => PurchaseOrderStatusExtensions.FromDbString(v));

        builder.Property(po => po.Notes).HasColumnName("notes").HasColumnType("nvarchar(max)");
        builder.Property(po => po.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(po => po.CreatedDate).HasColumnName("created_date").HasColumnType("datetime2").IsRequired();
        builder.Property(po => po.ModifiedDate).HasColumnName("modified_date").HasColumnType("datetime2").IsRequired();

        builder.HasMany(po => po.Lines)
            .WithOne()
            .HasForeignKey(l => l.PoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(po => po.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(po => po.TotalAmount);
        builder.Ignore(po => po.DomainEvents);
    }
}
