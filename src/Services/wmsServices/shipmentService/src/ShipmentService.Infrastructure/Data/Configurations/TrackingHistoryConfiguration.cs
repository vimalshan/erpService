using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Data.Configurations;

public class TrackingHistoryConfiguration : IEntityTypeConfiguration<TrackingHistory>
{
    public void Configure(EntityTypeBuilder<TrackingHistory> builder)
    {
        builder.ToTable("TrackingHistory");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("tracking_id").UseIdentityColumn();
        builder.Property(t => t.ShipmentId).HasColumnName("shipment_id").IsRequired();
        builder.Property(t => t.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        builder.Property(t => t.Location).HasColumnName("location").HasMaxLength(100);
        builder.Property(t => t.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(t => t.EventDatetime).HasColumnName("event_datetime");
        builder.Property(t => t.CreatedBy).HasColumnName("created_by").HasMaxLength(50);

        builder.HasIndex(t => t.ShipmentId).HasDatabaseName("IX_TrackingHistory_Shipment");
    }
}
