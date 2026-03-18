using LoanManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Infrastructure.Data.Configurations;

public class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepaymentSchedule>
{
    public void Configure(EntityTypeBuilder<LoanRepaymentSchedule> builder)
    {
        builder.ToTable("LOAN_REPAYSCH");

        builder.HasKey(x => x.RepayId);
        builder.Property(x => x.RepayId).HasColumnName("REPAY_ID");
        builder.Property(x => x.RepayLoanId).HasColumnName("REPAY_LOANID");
        builder.Property(x => x.RepayDate).HasColumnName("REPAY_DATE");
        builder.Property(x => x.RepayAmt).HasColumnName("REPAY_AMT").HasColumnType("decimal(19,0)");
        builder.Property(x => x.RepayFlag).HasColumnName("REPAY_FLAG").HasMaxLength(1);
        builder.Property(x => x.RepayModifiedOn).HasColumnName("REPAY_MODIFIEDON");
        builder.Property(x => x.RepayModifiedBy).HasColumnName("REPAY_MODIFIEDBY");

        builder.HasIndex(x => x.RepayLoanId).HasDatabaseName("IX_LOAN_REPAYSCH_LOANID");
        builder.HasIndex(x => x.RepayDate).HasDatabaseName("IX_LOAN_REPAYSCH_DATE");

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.IsOriginal);
        builder.Ignore(x => x.IsAmended);
    }
}
