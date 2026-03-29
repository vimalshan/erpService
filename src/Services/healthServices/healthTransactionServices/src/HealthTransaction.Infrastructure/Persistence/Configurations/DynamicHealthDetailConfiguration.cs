using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTransaction.Infrastructure.Persistence.Configurations;

public class DynamicHealthDetailConfiguration : IEntityTypeConfiguration<DynamicHealthDetail>
{
    public void Configure(EntityTypeBuilder<DynamicHealthDetail> builder)
    {
        builder.ToTable("HEALTH_DYN_DET");
        builder.HasKey(e => new { e.HlthNum, e.ChkupCod, e.ComCode, e.CtrlSrcId });

        builder.Property(e => e.HlthNum).HasColumnName("CDD_HLTH_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.ChkupCod).HasColumnName("CDD_CHKUP_COD").HasMaxLength(10);
        builder.Property(e => e.ComCode).HasColumnName("CDD_COM_COD").HasMaxLength(3);
        builder.Property(e => e.CtrlSrcId).HasColumnName("CDD_CTRLSRC_ID").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.DynVal).HasColumnName("CDD_DYN_VAL").HasMaxLength(500);
        builder.Property(e => e.EmpNum).HasColumnName("CDD_EMP_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.SysDate).HasColumnName("CDD_SYS_DAT").HasColumnType("DATE");
    }
}
