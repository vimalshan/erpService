using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLine");
        builder.HasKey(l => l.SoLineId);
        builder.Property(l => l.SoLineId).HasColumnName("so_line_id").ValueGeneratedOnAdd();
        builder.Property(l => l.SoId).HasColumnName("so_id").IsRequired();
        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(l => l.LineNumber).HasColumnName("line_number").IsRequired();
        builder.Property(l => l.QuantityOrdered).HasColumnName("quantity_ordered").HasColumnType("decimal(18,3)");
        builder.Property(l => l.QuantityShipped).HasColumnName("quantity_shipped").HasColumnType("decimal(18,3)").HasDefaultValue(0);
        builder.Property(l => l.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,4)");
        builder.Property(l => l.Notes).HasColumnName("notes");

        builder.HasIndex(l => new { l.SoId, l.LineNumber }).IsUnique().HasDatabaseName("UQ_SOLine_SO_Line");
        builder.HasIndex(l => l.ProductId).HasDatabaseName("IX_SalesOrderLine_Product");

        builder.Ignore(l => l.DomainEvents);
    }
}
