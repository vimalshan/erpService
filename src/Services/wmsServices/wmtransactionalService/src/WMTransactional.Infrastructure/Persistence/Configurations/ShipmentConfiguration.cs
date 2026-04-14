using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipment");
        builder.HasKey(s => s.ShipmentId);
        builder.Property(s => s.ShipmentId).HasColumnName("shipment_id").ValueGeneratedOnAdd();
        builder.Property(s => s.ShipmentNumber).HasColumnName("shipment_number").IsRequired().HasMaxLength(50);
        builder.Property(s => s.SoId).HasColumnName("so_id").IsRequired();
        builder.Property(s => s.ShippedDate).HasColumnName("shipped_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");
        builder.Property(s => s.Status).HasColumnName("status").IsRequired().HasMaxLength(30);
        builder.Property(s => s.TrackingNumber).HasColumnName("tracking_number").HasMaxLength(100);
        builder.Property(s => s.Carrier).HasColumnName("carrier").HasMaxLength(50);
        builder.Property(s => s.Notes).HasColumnName("notes");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.CreatedDate).HasColumnName("created_date").HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

        builder.HasIndex(s => s.ShipmentNumber).IsUnique();
        builder.HasIndex(s => s.SoId).HasDatabaseName("IX_Shipment_SO");

        builder.HasOne(s => s.SalesOrder).WithMany().HasForeignKey(s => s.SoId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Lines)
            .WithOne(l => l.Shipment)
            .HasForeignKey(l => l.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(s => s.DomainEvents);
    }
}
