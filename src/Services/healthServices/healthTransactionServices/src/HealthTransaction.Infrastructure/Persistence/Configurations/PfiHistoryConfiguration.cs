using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTransaction.Infrastructure.Persistence.Configurations;

public class PfiHistoryConfiguration : IEntityTypeConfiguration<PfiHistory>
{
    public void Configure(EntityTypeBuilder<PfiHistory> builder)
    {
        builder.ToTable("CHKUP_PFI_HIST");
        builder.HasKey(e => new { e.HlthNum, e.SympId });

        builder.Property(e => e.HlthNum).HasColumnName("CPH_HLTH_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.EmpNum).HasColumnName("CPH_EMP_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.SympId).HasColumnName("CPH_SYMP_ID").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.YnFlag).HasColumnName("CPH_YN_FLAG").HasColumnType("CHAR(1)").HasConversion(
            v => v.HasValue ? v.Value.ToString() : null,
            v => string.IsNullOrEmpty(v) ? null : v[0]);
        builder.Property(e => e.ImmDate).HasColumnName("CPH_IMM_DAT").HasColumnType("DATE");
        builder.Property(e => e.TestValue).HasColumnName("CPH_TEST_VAL").HasMaxLength(100);
    }
}
