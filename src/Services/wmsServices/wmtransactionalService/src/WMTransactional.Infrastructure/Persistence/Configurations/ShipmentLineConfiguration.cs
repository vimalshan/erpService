using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class ShipmentLineConfiguration : IEntityTypeConfiguration<ShipmentLine>
{
    public void Configure(EntityTypeBuilder<ShipmentLine> builder)
    {
        builder.ToTable("ShipmentLine");
        builder.HasKey(l => l.ShipmentLineId);
        builder.Property(l => l.ShipmentLineId).HasColumnName("shipment_line_id").ValueGeneratedOnAdd();
        builder.Property(l => l.ShipmentId).HasColumnName("shipment_id").IsRequired();
        builder.Property(l => l.SoLineId).HasColumnName("so_line_id").IsRequired();
        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(l => l.BinId).HasColumnName("bin_id").IsRequired();
        builder.Property(l => l.QuantityShipped).HasColumnName("quantity_shipped").HasColumnType("decimal(18,3)");
        builder.Property(l => l.LotNumber).HasColumnName("lot_number").HasMaxLength(50);
        builder.Property(l => l.ExpiryDate).HasColumnName("expiry_date").HasColumnType("date");
        builder.Property(l => l.Notes).HasColumnName("notes");

        builder.HasIndex(l => l.SoLineId).HasDatabaseName("IX_ShipmentLine_SOLine");
        builder.HasIndex(l => l.ProductId).HasDatabaseName("IX_ShipmentLine_Product");
        builder.HasIndex(l => l.BinId).HasDatabaseName("IX_ShipmentLine_Bin");

        builder.Ignore(l => l.DomainEvents);
    }
}
