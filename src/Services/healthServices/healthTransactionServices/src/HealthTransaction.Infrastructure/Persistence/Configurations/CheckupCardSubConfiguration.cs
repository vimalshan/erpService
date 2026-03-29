using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTransaction.Infrastructure.Persistence.Configurations;

public class CheckupCardSubConfiguration : IEntityTypeConfiguration<CheckupCardSub>
{
    public void Configure(EntityTypeBuilder<CheckupCardSub> builder)
    {
        builder.ToTable("HLTH_CHKCARD_SUB");
        builder.HasKey(e => new { e.HlthNum, e.SympId });

        builder.Property(e => e.HlthNum).HasColumnName("HCS_HLTH_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.SympId).HasColumnName("HCS_SYMP_ID").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.FlagYn).HasColumnName("HCS_FLAG_YN").HasColumnType("CHAR(1)").HasConversion(
            v => v.HasValue ? v.Value.ToString() : null,
            v => string.IsNullOrEmpty(v) ? null : v[0]);
        builder.Property(e => e.SympVal).HasColumnName("HCS_SYMP_VAL").HasMaxLength(200);
        builder.Property(e => e.EmpNum).HasColumnName("HCS_EMP_NUM").HasColumnType("NUMERIC(10,0)");

        builder.Ignore(e => e.DomainEvents);
    }
}
