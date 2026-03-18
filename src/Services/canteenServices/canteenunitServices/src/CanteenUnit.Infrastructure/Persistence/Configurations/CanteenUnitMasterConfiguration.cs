using CanteenUnit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CanteenUnit.Infrastructure.Persistence.Configurations;

public class CanteenUnitMasterConfiguration : IEntityTypeConfiguration<CanteenUnitMaster>
{
    public void Configure(EntityTypeBuilder<CanteenUnitMaster> builder)
    {
        builder.ToTable("CANTEEN_UNIT_MASTER");
        builder.HasKey(e => e.UnComCod);
        builder.Property(e => e.UnComCod).HasColumnName("UN_COM_COD").HasColumnType("DECIMAL(38,0)").IsRequired();
        builder.Property(e => e.UnUntName).HasColumnName("UN_UNT_NAME").HasMaxLength(100);
        builder.Property(e => e.UntUntRef).HasColumnName("UNT_UNT_REF").HasMaxLength(100);
        builder.Property(e => e.UnMaxVal).HasColumnName("UN_MAX_VAL").HasColumnType("DECIMAL(38,0)");
        builder.Property(e => e.InMinVal).HasColumnName("IN_MIN_VAL").HasColumnType("DECIMAL(38,0)");
        builder.Property(e => e.UnSitId).HasColumnName("UN_SIT_ID");
        builder.Property(e => e.UnHrmsId).HasColumnName("UN_HRMS_ID");

        builder.Ignore(e => e.Accesses);
    }
}
