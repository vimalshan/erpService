using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivingService.Domain.Entities;

namespace ReceivingService.Infrastructure.Data.Configurations;

public sealed class ReceivingLineConfiguration
    : IEntityTypeConfiguration<ReceivingLine>
{
    public void Configure(EntityTypeBuilder<ReceivingLine> builder)
    {
        builder.ToTable("ReceivingLine");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
               .HasColumnName("receiving_line_id")
               .UseIdentityColumn();

        builder.Property(l => l.ReceivingId)
               .HasColumnName("receiving_id")
               .IsRequired();

        builder.HasIndex(l => l.ReceivingId)
               .HasDatabaseName("IX_ReceivingLine_Receiving");

        builder.Property(l => l.PoLineId)
               .HasColumnName("po_line_id")
               .IsRequired();

        builder.HasIndex(l => l.PoLineId)
               .HasDatabaseName("IX_ReceivingLine_POLine");

        builder.Property(l => l.ProductId)
               .HasColumnName("product_id")
               .IsRequired();

        builder.HasIndex(l => l.ProductId)
               .HasDatabaseName("IX_ReceivingLine_Product");

        builder.Property(l => l.BinId)
               .HasColumnName("bin_id")
               .IsRequired();

        builder.HasIndex(l => l.BinId)
               .HasDatabaseName("IX_ReceivingLine_Bin");

        builder.Property(l => l.QuantityReceived)
               .HasColumnName("quantity_received")
               .HasPrecision(18, 3)
               .IsRequired();

        builder.Property(l => l.LotNumber)
               .HasColumnName("lot_number")
               .HasMaxLength(50);

        builder.Property(l => l.ExpiryDate)
               .HasColumnName("expiry_date");

        builder.Property(l => l.Notes)
               .HasColumnName("notes");

        builder.Ignore(l => l.DomainEvents);
    }
}
