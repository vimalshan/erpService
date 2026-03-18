using LoanManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Infrastructure.Data.Configurations;

public class LoanDisbursementConfiguration : IEntityTypeConfiguration<LoanDisbursementSchedule>
{
    public void Configure(EntityTypeBuilder<LoanDisbursementSchedule> builder)
    {
        builder.ToTable("LOAN_DISBSCH");

        builder.HasKey(x => x.DisbId);
        builder.Property(x => x.DisbId).HasColumnName("DISB_ID");
        builder.Property(x => x.DisbLoanId).HasColumnName("DISB_LOANID");
        builder.Property(x => x.DisbDate).HasColumnName("DISB_DATE");
        builder.Property(x => x.DisbAmount).HasColumnName("DISB_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(x => x.DisbExcRate).HasColumnName("DISB_EXCRATE").HasColumnType("decimal(19,0)");
        builder.Property(x => x.DisbExcAmt).HasColumnName("DISB_EXCAMT").HasColumnType("decimal(19,0)");
        builder.Property(x => x.DisbModifiedBy).HasColumnName("DISB_MODIFIEDBY");
        builder.Property(x => x.DisbModifiedOn).HasColumnName("DISB_MODIFIEDON");

        builder.HasIndex(x => x.DisbLoanId).HasDatabaseName("IX_LOAN_DISBSCH_LOANID");
        builder.HasIndex(x => x.DisbDate).HasDatabaseName("IX_LOAN_DISBSCH_DATE");

        builder.Ignore(x => x.DomainEvents);
    }
}
