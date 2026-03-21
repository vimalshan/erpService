using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentService.Domain.Entities;

namespace ShipmentService.Infrastructure.Data.Configurations;

public class DeliveryAttemptConfiguration : IEntityTypeConfiguration<DeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<DeliveryAttempt> builder)
    {
        builder.ToTable("DeliveryAttempt");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("attempt_id").UseIdentityColumn();
        builder.Property(d => d.ShipmentId).HasColumnName("shipment_id").IsRequired();
        builder.Property(d => d.AttemptDate).HasColumnName("attempt_date").IsRequired();
        builder.Property(d => d.Result).HasColumnName("result").HasMaxLength(20).HasConversion<string>();
        builder.Property(d => d.Reason).HasColumnName("reason").HasMaxLength(255);
        builder.Property(d => d.Notes).HasColumnName("notes");
    }
}
