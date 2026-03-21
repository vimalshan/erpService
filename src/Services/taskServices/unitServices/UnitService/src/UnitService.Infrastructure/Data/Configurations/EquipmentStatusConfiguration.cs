using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;

namespace UnitService.Infrastructure.Data.Configurations;

public class EquipmentStatusConfiguration : IEntityTypeConfiguration<EquipmentStatus>
{
    public void Configure(EntityTypeBuilder<EquipmentStatus> builder)
    {
        builder.ToTable("UM_EQUIP_STATUS");

        builder.HasKey(e => e.StatusId);
        builder.Property(e => e.StatusId).HasColumnName("ES_ID").ValueGeneratedNever();
        builder.Property(e => e.EquipmentId).HasColumnName("ES_EQUIPMENT_ID").IsRequired();
        builder.Property(e => e.StatusDescription).HasColumnName("ES_STATUS_DESC").HasMaxLength(65).IsRequired();
        builder.Property(e => e.StatusCode).HasColumnName("ES_STATUS_ID").HasMaxLength(5).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("ES_START_DATE").HasMaxLength(255).IsRequired();
        builder.Property(e => e.CloseDate).HasColumnName("ES_CLOSE_DATE").HasMaxLength(255);
        builder.Property(e => e.Remarks).HasColumnName("ES_REMARKS").HasMaxLength(200);
        builder.Property(e => e.Hours).HasColumnName("ES_HOURS");
        builder.Property(e => e.FilePath).HasColumnName("ES_FILEPATH").HasMaxLength(100);
        builder.Property(e => e.CreatedBy).HasColumnName("ES_CREATED_BY");
        builder.Property(e => e.CreatedOn).HasColumnName("ES_CREATED_ON").HasMaxLength(255);
        builder.Property(e => e.LastModifiedBy).HasColumnName("ES_LAST_MODIFIED_BY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("ES_LAST_MODIFIED_ON").HasPrecision(3);

        builder.HasOne(e => e.Equipment)
            .WithMany(eq => eq.Statuses)
            .HasForeignKey(e => e.EquipmentId);
    }
}
