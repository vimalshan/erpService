using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.ToTable("PurchaseOrderLine");
        builder.HasKey(l => l.PoLineId);
        builder.Property(l => l.PoLineId).HasColumnName("po_line_id").ValueGeneratedOnAdd();
        builder.Property(l => l.PoId).HasColumnName("po_id").IsRequired();
        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(l => l.LineNumber).HasColumnName("line_number").IsRequired();
        builder.Property(l => l.QuantityOrdered).HasColumnName("quantity_ordered").HasColumnType("decimal(18,3)");
        builder.Property(l => l.QuantityReceived).HasColumnName("quantity_received").HasColumnType("decimal(18,3)").HasDefaultValue(0);
        builder.Property(l => l.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,4)");
        builder.Property(l => l.Notes).HasColumnName("notes");

        builder.HasIndex(l => new { l.PoId, l.LineNumber }).IsUnique().HasDatabaseName("UQ_POLine_PO_Line");
        builder.HasIndex(l => l.ProductId).HasDatabaseName("IX_PurchaseOrderLine_Product");

        builder.Ignore(l => l.DomainEvents);
    }
}
