using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public class TravelAccountConfiguration : IEntityTypeConfiguration<TravelAccount>
{
    public void Configure(EntityTypeBuilder<TravelAccount> builder)
    {
        builder.ToTable("TRAVEL_ACCOUNT");
        builder.HasKey(e => e.TransactionNumber);
        builder.Property(e => e.TransactionNumber).HasColumnName("AC_TRN_NUM").ValueGeneratedNever();
        builder.Property(e => e.UnitCode).HasColumnName("AC_UNT_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.UserCode).HasColumnName("AC_USR_COD").HasMaxLength(20);
        builder.Property(e => e.UserNumber).HasColumnName("AC_USR_NUM");
        builder.Property(e => e.DebitCreditFlag).HasColumnName("AC_DC_FLG").HasMaxLength(1).IsFixedLength();
        builder.Property(e => e.TransactionAmount).HasColumnName("AC_TRN_AMT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.AccountCode).HasColumnName("AC_ACC_COD").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.Remarks).HasColumnName("AC_REM_MRK").HasMaxLength(200);
        builder.Property(e => e.AccountType).HasColumnName("AC_ACC_TYP").HasMaxLength(3).IsFixedLength();
        builder.Property(e => e.JvPostingStatus).HasColumnName("AC_JV_STS").HasMaxLength(1).IsFixedLength();
    }
}
