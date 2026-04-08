using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore.Configurations;

public class FinancialYearConfiguration : IEntityTypeConfiguration<FinancialYear>
{
    public void Configure(EntityTypeBuilder<FinancialYear> builder)
    {
        builder.ToTable("COMP_FINYEAR");
        builder.HasKey(e => e.AcSrlNum);

        builder.Property(e => e.AcSrlNum).HasColumnName("AC_SRL_NUM").ValueGeneratedNever();
        builder.Property(e => e.AcStrDat).HasColumnName("AC_STR_DAT").HasPrecision(3);
        builder.Property(e => e.AcEndDat).HasColumnName("AC_END_DAT").HasPrecision(3);
        builder.Property(e => e.AcClsFlg).HasColumnName("AC_CLS_FLG")
            .HasConversion(
                v => ((char)v).ToString(),
                v => (FinancialYearStatus)v[0])
            .HasMaxLength(1);
        builder.Property(e => e.AcRemarks).HasColumnName("AC_REMARKS").HasMaxLength(4000);
        builder.Property(e => e.AcIntFlg).HasColumnName("AC_INT_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.AcEmpName).HasColumnName("AC_EMP_NAME").HasMaxLength(65);
        builder.Property(e => e.AcEmpDesg).HasColumnName("AC_EMP_DESG").HasMaxLength(65);
        builder.Property(e => e.AcBatNum).HasColumnName("AC_BAT_NUM");

        builder.Ignore(e => e.DomainEvents);
    }
}
