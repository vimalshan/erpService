using HealthTransaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTransaction.Infrastructure.Persistence.Configurations;

public class PreEmploymentCheckupConfiguration : IEntityTypeConfiguration<PreEmploymentCheckup>
{
    public void Configure(EntityTypeBuilder<PreEmploymentCheckup> builder)
    {
        builder.ToTable("CHKUP_PRE_MAIN");
        builder.HasKey(e => new { e.EmpNum, e.ComCode });

        builder.Property(e => e.EmpNum).HasColumnName("CPM_EMP_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.ComCode).HasColumnName("CPM_COM_COD").HasMaxLength(3);
        builder.Property(e => e.HlthNum).HasColumnName("CPM_HLTH_NUM").HasColumnType("NUMERIC(10,0)");
        builder.Property(e => e.PhysHandicap).HasColumnName("CPM_PHYS_HAND").HasMaxLength(1);
        builder.Property(e => e.ProposedEmp).HasColumnName("CPM_PROP_EMP").HasMaxLength(50);
        builder.Property(e => e.IdentMarks).HasColumnName("CPM_IDENT_MARKS").HasMaxLength(200);
        builder.Property(e => e.FinalRemarks).HasColumnName("CPM_FINAL_RMKS").HasMaxLength(500);
        builder.Property(e => e.FitPh).HasColumnName("CPM_FIT_PH").HasColumnType("CHAR(3)").HasConversion(
            v => v.HasValue ? v.Value.ToString() : null,
            v => string.IsNullOrEmpty(v) ? null : v[0]);
        builder.Property(e => e.FitFinal).HasColumnName("CPM_FIT_FINAL").HasMaxLength(1);
        builder.Property(e => e.CheckupDate).HasColumnName("CPM_CHK_DAT").HasColumnType("DATE");
    }
}
