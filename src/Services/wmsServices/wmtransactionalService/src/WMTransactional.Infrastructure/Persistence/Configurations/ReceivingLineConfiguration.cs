using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class ReceivingLineConfiguration : IEntityTypeConfiguration<ReceivingLine>
{
    public void Configure(EntityTypeBuilder<ReceivingLine> builder)
    {
        builder.ToTable("ReceivingLine");
        builder.HasKey(l => l.ReceivingLineId);
        builder.Property(l => l.ReceivingLineId).HasColumnName("receiving_line_id").ValueGeneratedOnAdd();
        builder.Property(l => l.ReceivingId).HasColumnName("receiving_id").IsRequired();
        builder.Property(l => l.PoLineId).HasColumnName("po_line_id").IsRequired();
        builder.Property(l => l.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(l => l.BinId).HasColumnName("bin_id").IsRequired();
        builder.Property(l => l.QuantityReceived).HasColumnName("quantity_received").HasColumnType("decimal(18,3)");
        builder.Property(l => l.LotNumber).HasColumnName("lot_number").HasMaxLength(50);
        builder.Property(l => l.ExpiryDate).HasColumnName("expiry_date").HasColumnType("date");
        builder.Property(l => l.Notes).HasColumnName("notes");

        builder.HasIndex(l => l.PoLineId).HasDatabaseName("IX_ReceivingLine_POLine");
        builder.HasIndex(l => l.ProductId).HasDatabaseName("IX_ReceivingLine_Product");
        builder.HasIndex(l => l.BinId).HasDatabaseName("IX_ReceivingLine_Bin");

        builder.Ignore(l => l.DomainEvents);
    }
}
