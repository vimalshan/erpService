using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Data.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("Package");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("package_id").UseIdentityColumn();
        builder.Property(p => p.ShipmentId).HasColumnName("shipment_id").IsRequired();
        builder.Property(p => p.PackageNumber).HasColumnName("package_number").HasMaxLength(20).IsRequired();
        builder.Property(p => p.Weight).HasColumnName("weight").HasColumnType("decimal(10,2)");
        builder.Property(p => p.Volume).HasColumnName("volume").HasColumnType("decimal(10,2)");
        builder.Property(p => p.Dimensions).HasColumnName("dimensions").HasMaxLength(50);
        builder.Property(p => p.TrackingNumber).HasColumnName("tracking_number").HasMaxLength(50);
        builder.Property(p => p.ContentsDescription).HasColumnName("contents_description").HasMaxLength(255);

        builder.HasIndex(p => p.ShipmentId).HasDatabaseName("IX_Package_Shipment");
        builder.HasAlternateKey(p => new { p.ShipmentId, p.PackageNumber }).HasName("UQ_Package_Shipment");
    }
}
