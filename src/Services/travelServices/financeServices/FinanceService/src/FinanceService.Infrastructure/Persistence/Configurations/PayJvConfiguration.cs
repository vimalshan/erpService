using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class PayJvConfiguration : IEntityTypeConfiguration<PayJv>
{
    public void Configure(EntityTypeBuilder<PayJv> builder)
    {
        builder.ToTable("PAY_JV");
        builder.HasKey(e => new { e.SerialNumber, e.FinancialYear, e.DocumentNumber, e.CompanyCode });
        builder.Property(e => e.CompanyCode).HasColumnName("AC_ENT_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.FinancialYear).HasColumnName("AC_FIN_YER");
        builder.Property(e => e.DocumentNumber).HasColumnName("AC_DOC_NUM");
        builder.Property(e => e.SerialNumber).HasColumnName("AC_SRL_NUM");
        builder.Property(e => e.PayBatchNo).HasColumnName("AC_PAY_NUM");
        builder.Property(e => e.PayDate).HasColumnName("AC_PAY_DAT");
        builder.Property(e => e.AccountCode).HasColumnName("AC_ACC_COD").HasMaxLength(6).IsFixedLength();
        builder.Property(e => e.TransactionAmount).HasColumnName("AC_TRN_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Narration).HasColumnName("AC_NAR_TON").HasMaxLength(200);
        builder.Property(e => e.PostingFlag).HasColumnName("AC_PST_TYP").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.EnteredOn).HasColumnName("AC_ENT_DAT");
        builder.Property(e => e.CancelledOn).HasColumnName("AC_CAN_DAT");
        builder.Property(e => e.EnteredBy).HasColumnName("AC_ENT_USR").HasMaxLength(25);
    }
}
