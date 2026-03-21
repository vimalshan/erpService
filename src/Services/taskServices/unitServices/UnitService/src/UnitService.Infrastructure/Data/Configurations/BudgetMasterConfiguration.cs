using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnitService.Domain.Entities;
using UnitService.Domain.ValueObjects;

namespace UnitService.Infrastructure.Data.Configurations;

public class BudgetMasterConfiguration : IEntityTypeConfiguration<BudgetMaster>
{
    public void Configure(EntityTypeBuilder<BudgetMaster> builder)
    {
        builder.ToTable("UM_BUDGET_MASTER");

        builder.HasKey(e => e.UnitCode);
        builder.Property(e => e.UnitCode).HasColumnName("BM_UNIT_CODE").HasMaxLength(3).IsRequired()
            .HasConversion(v => v.Value, v => UnitCode.From(v));
        builder.Property(e => e.EquipmentId).HasColumnName("BM_EQUIPMENT_ID").HasColumnType("decimal(38,0)");
        builder.Property(e => e.StartDate).HasColumnName("BM_START_DATE").HasPrecision(3).IsRequired();
        builder.Property(e => e.CloseDate).HasColumnName("BM_CLOSE_DATE").HasPrecision(3);
        builder.Property(e => e.LastModifiedBy).HasColumnName("BM_LAST_MODIFIEDBY");
        builder.Property(e => e.LastModifiedOn).HasColumnName("BM_LAST_MODIFIEDON").HasPrecision(3);
    }
}
