using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PurchaseOrderService.Domain.Entities;

namespace PurchaseOrderService.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.ToTable("PurchaseOrderLine");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("po_line_id").UseIdentityColumn();

        builder.Property(l => l.PoId).HasColumnName("po_id").IsRequired();
        builder.HasIndex(l => l.PoId);

        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.HasIndex(l => l.ProductId);

        builder.Property(l => l.LineNumber).HasColumnName("line_number").IsRequired();
        builder.Property(l => l.QuantityOrdered).HasColumnName("quantity_ordered").HasColumnType("decimal(18,3)").IsRequired();
        builder.Property(l => l.QuantityReceived).HasColumnName("quantity_received").HasColumnType("decimal(18,3)").IsRequired();
        builder.Property(l => l.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,4)");
        builder.Property(l => l.Notes).HasColumnName("notes").HasColumnType("nvarchar(max)");

        builder.HasIndex(l => new { l.PoId, l.LineNumber }).IsUnique();

        builder.Ignore(l => l.IsFullyReceived);
        builder.Ignore(l => l.LineTotal);
        builder.Ignore(l => l.DomainEvents);
    }
}
