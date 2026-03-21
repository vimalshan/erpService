using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Data.Configurations;

public class ShipmentLineConfiguration : IEntityTypeConfiguration<ShipmentLine>
{
    public void Configure(EntityTypeBuilder<ShipmentLine> builder)
    {
        builder.ToTable("ShipmentLine");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("shipment_line_id").UseIdentityColumn();
        builder.Property(l => l.ShipmentId).HasColumnName("shipment_id").IsRequired();
        builder.Property(l => l.SoLineId).HasColumnName("so_line_id");
        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(l => l.BinId).HasColumnName("bin_id").IsRequired();
        builder.Property(l => l.QuantityShipped).HasColumnName("quantity_shipped").HasColumnType("decimal(18,3)").IsRequired();
        builder.Property(l => l.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(18,4)");
        builder.Property(l => l.LotNumber).HasColumnName("lot_number").HasMaxLength(50);
        builder.Property(l => l.ExpiryDate).HasColumnName("expiry_date");
        builder.Property(l => l.Notes).HasColumnName("notes");

        builder.HasIndex(l => l.ShipmentId).HasDatabaseName("IX_ShipmentLine_Shipment");
        builder.HasIndex(l => l.SoLineId).HasDatabaseName("IX_ShipmentLine_SOLine");
        builder.HasIndex(l => l.ProductId).HasDatabaseName("IX_ShipmentLine_Product");
        builder.HasIndex(l => l.BinId).HasDatabaseName("IX_ShipmentLine_Bin");
    }
}
