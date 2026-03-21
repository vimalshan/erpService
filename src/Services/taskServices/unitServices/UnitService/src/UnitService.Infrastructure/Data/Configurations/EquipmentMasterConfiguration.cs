using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class EquipmentMasterConfiguration : IEntityTypeConfiguration<EquipmentMaster>
{
    public void Configure(EntityTypeBuilder<EquipmentMaster> builder)
    {
        builder.ToTable("UM_EQUIPMENT_MASTER");

        builder.HasKey(e => e.EquipmentId);
        builder.Property(e => e.EquipmentId).HasColumnName("EM_EQUIPMENT_ID").ValueGeneratedNever();
        builder.Property(e => e.EquipmentName).HasColumnName("EM_EQUIPMENT_NAME").HasMaxLength(65).IsRequired();
        builder.Property(e => e.UnitCode).HasColumnName("EM_UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.Category).HasColumnName("EM_CATEGORY").HasMaxLength(25).IsRequired();
        builder.Property(e => e.StartDate).HasColumnName("EM_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.CloseDate).HasColumnName("EM_CLOSE_DATE").HasPrecision(3);
        builder.Property(e => e.LastModifiedBy).HasColumnName("EM_LAST_MODIFIEDBY").IsRequired();
        builder.Property(e => e.LastModifiedOn).HasColumnName("EM_LAST_MODIFIEDON").HasPrecision(3).IsRequired();

        builder.HasMany(e => e.Statuses)
            .WithOne(s => s.Equipment)
            .HasForeignKey(s => s.EquipmentId);
    }
}
