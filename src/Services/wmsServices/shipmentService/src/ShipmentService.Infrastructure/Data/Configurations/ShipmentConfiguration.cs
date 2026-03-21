using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Data.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipment");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("shipment_id").UseIdentityColumn();

        builder.Property(s => s.ShipmentNumber).HasColumnName("shipment_number").HasMaxLength(50).IsRequired();
        builder.HasIndex(s => s.ShipmentNumber).IsUnique().HasDatabaseName("IX_Shipment_ShipmentNumber");

        builder.Property(s => s.SoId).HasColumnName("so_id");
        builder.Property(s => s.CustomerId).HasColumnName("customer_id").IsRequired();
        builder.Property(s => s.WarehouseId).HasColumnName("warehouse_id").IsRequired();
        builder.Property(s => s.ShipmentType).HasColumnName("shipment_type").HasMaxLength(20)
            .HasConversion<string>();
        builder.Property(s => s.ServiceType).HasColumnName("service_type").HasMaxLength(20);
        builder.Property(s => s.ShippedDate).HasColumnName("shipped_date");
        builder.Property(s => s.Status).HasColumnName("status").HasMaxLength(30)
            .HasConversion<string>();
        builder.Property(s => s.TrackingNumber).HasColumnName("tracking_number").HasMaxLength(100);
        builder.Property(s => s.Carrier).HasColumnName("carrier").HasMaxLength(50);
        builder.Property(s => s.TotalWeight).HasColumnName("total_weight").HasColumnType("decimal(18,3)");
        builder.Property(s => s.TotalVolume).HasColumnName("total_volume").HasColumnType("decimal(18,3)");
        builder.Property(s => s.SpecialInstructions).HasColumnName("special_instructions");
        builder.Property(s => s.Notes).HasColumnName("notes");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(50);
        builder.Property(s => s.CreatedDate).HasColumnName("created_date");
        builder.Property(s => s.ModifiedDate).HasColumnName("modified_date");

        builder.HasIndex(s => s.SoId).HasDatabaseName("IX_Shipment_SO");
        builder.HasIndex(s => s.CustomerId).HasDatabaseName("IX_Shipment_Customer");
        builder.HasIndex(s => s.WarehouseId).HasDatabaseName("IX_Shipment_Warehouse");
        builder.HasIndex(s => s.TrackingNumber).HasFilter("tracking_number IS NOT NULL")
            .HasDatabaseName("IX_Shipment_TrackingNumber");

        builder.HasMany(s => s.Lines)
            .WithOne(l => l.Shipment)
            .HasForeignKey(l => l.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Packages)
            .WithOne(p => p.Shipment)
            .HasForeignKey(p => p.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.TrackingHistory)
            .WithOne(t => t.Shipment)
            .HasForeignKey(t => t.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.DeliveryAttempts)
            .WithOne(d => d.Shipment)
            .HasForeignKey(d => d.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(s => s.Packages).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(s => s.TrackingHistory).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(s => s.DeliveryAttempts).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
