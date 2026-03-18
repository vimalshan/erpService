using LoanManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Infrastructure.Data.Configurations;

public class LoanMainConfiguration : IEntityTypeConfiguration<LoanMain>
{
    public void Configure(EntityTypeBuilder<LoanMain> builder)
    {
        builder.ToTable("LOAN_MAIN");

        builder.HasKey(x => x.LoanId);
        builder.Property(x => x.LoanId).HasColumnName("LOAN_ID").HasColumnType("decimal(38,0)");
        builder.Property(x => x.LoanKey).HasColumnName("LOAN_KEY").HasMaxLength(15).IsRequired();
        builder.Property(x => x.LoanOrgId).HasColumnName("LOAN_ORGID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LoanOrgCurr).HasColumnName("LOAN_ORGCURR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.LoanCurr).HasColumnName("LOAN_CURR").HasColumnType("decimal(38,0)");
        builder.Property(x => x.LoanDate).HasColumnName("LOAN_DATE").IsRequired();
        builder.Property(x => x.LoanTypeId).HasColumnName("LOAN_TYPEID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LoanBankId).HasColumnName("LOAN_BANKID").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LoanCreatedBy).HasColumnName("LOAN_CREATEDBY").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LoanCreatedOn).HasColumnName("LOAN_CREATEDON").IsRequired();
        builder.Property(x => x.LoanModifiedBy).HasColumnName("LOAN_MODIFIEDBY").HasColumnType("decimal(38,0)");
        builder.Property(x => x.LoanModifiedOn).HasColumnName("LOAN_MODIFIEDON");
        builder.Property(x => x.LoanAmount).HasColumnName("LOAN_AMOUNT").HasColumnType("decimal(38,0)").IsRequired();
        builder.Property(x => x.LoanStatus).HasColumnName("LOAN_STATUS").HasMaxLength(1);

        builder.HasIndex(x => x.LoanOrgId).HasDatabaseName("IX_LOAN_MAIN_ORGID");
        builder.HasIndex(x => x.LoanDate).HasDatabaseName("IX_LOAN_MAIN_DATE");

        builder.HasMany(x => x.Disbursements)
            .WithOne()
            .HasForeignKey("DisbLoanId")
            .HasConstraintName("FK_LOAN_DISBSCH_MAIN");

        builder.HasMany(x => x.Interests)
            .WithOne()
            .HasForeignKey("IntLoanId")
            .HasConstraintName("FK_LOAN_INTEREST_MAIN");

        builder.HasMany(x => x.Repayments)
            .WithOne()
            .HasForeignKey("RepayLoanId")
            .HasConstraintName("FK_LOAN_REPAYSCH_MAIN");

        builder.Ignore(x => x.DomainEvents);
    }
}
