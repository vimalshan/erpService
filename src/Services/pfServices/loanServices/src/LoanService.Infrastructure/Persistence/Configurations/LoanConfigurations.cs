using LoanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanService.Infrastructure.Persistence.Configurations;

public class LoanMainConfiguration : IEntityTypeConfiguration<LoanMain>
{
    public void Configure(EntityTypeBuilder<LoanMain> builder)
    {
        builder.ToTable("LOAN_MAIN");
        builder.HasKey(e => e.LoanNo);

        builder.Property(e => e.LoanNo).HasColumnName("LOAN_NO").ValueGeneratedNever();
        builder.Property(e => e.TrustCode).HasColumnName("LOAN_TRUST_CODE").HasColumnType("char(3)");
        builder.Property(e => e.MemberId).HasColumnName("LOAN_MEMBER_ID");
        builder.Property(e => e.LoanDate).HasColumnName("LOAN_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.LoanAmount).HasColumnName("LOAN_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.LoanType).HasColumnName("LOAN_TYPE");
        builder.Property(e => e.LoanReason).HasColumnName("LOAN_REASON").HasMaxLength(200);
        builder.Property(e => e.LoanTenure).HasColumnName("LOAN_TENURE").HasMaxLength(10);
        builder.Property(e => e.PrincipalOutstanding).HasColumnName("LOAN_PRINCIPALOS").HasColumnType("decimal(19,0)");
        builder.Property(e => e.ClsFlag).HasColumnName("LOAN_CLSFLAG").HasColumnType("char(1)");
        builder.Property(e => e.UpdatedByEmpId).HasColumnName("LOAN_UPDBY_EMP_SYSIDC");
        builder.Property(e => e.UpdatedOn).HasColumnName("LOAN_UPDON").HasColumnType("datetime2(3)");
        builder.Property(e => e.Status).HasColumnName("LOAN_STATUS").HasColumnType("char(1)").HasDefaultValue('A');
        builder.Property(e => e.Rate).HasColumnName("LOAN_RATE").HasColumnType("decimal(5,2)");
        builder.Property(e => e.ApprovalDate).HasColumnName("LOAN_APPROVAL_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.ClosureDate).HasColumnName("LOAN_CLOSURE_DATE").HasColumnType("datetime2(3)");

        builder.HasMany(e => e.Repayments)
            .WithOne(e => e.Loan)
            .HasForeignKey(e => e.LoanNo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Deductions)
            .WithOne(e => e.Loan)
            .HasForeignKey(e => e.LoanNo)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.MemberId).HasDatabaseName("IDX_LOAN_MAIN_MEMBER");

        builder.Ignore(e => e.DomainEvents);
        builder.Ignore(e => e.Version);
    }
}

public class LoanRepaymentConfiguration : IEntityTypeConfiguration<LoanRepayment>
{
    public void Configure(EntityTypeBuilder<LoanRepayment> builder)
    {
        builder.ToTable("LOAN_REPAYMENT");
        builder.HasKey(e => e.RepayId);

        builder.Property(e => e.RepayId).HasColumnName("REPAY_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.LoanNo).HasColumnName("LOAN_NO");
        builder.Property(e => e.InstallmentNo).HasColumnName("REPAY_INSTALLMENT_NO");
        builder.Property(e => e.RepayAmount).HasColumnName("REPAY_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.DueDate).HasColumnName("REPAY_DUE_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.PaidDate).HasColumnName("REPAY_PAID_DATE").HasColumnType("datetime2(3)");
        builder.Property(e => e.PaidAmount).HasColumnName("REPAY_PAID_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.Status).HasColumnName("REPAY_STATUS").HasColumnType("char(1)").HasDefaultValue('O');

        builder.HasIndex(new[] { "LoanNo", "Status" }).HasDatabaseName("IDX_LOAN_REPAYMENT_LOAN");

        builder.Ignore(e => e.DomainEvents);
    }
}

public class LoanDeductionConfiguration : IEntityTypeConfiguration<LoanDeduction>
{
    public void Configure(EntityTypeBuilder<LoanDeduction> builder)
    {
        builder.ToTable("LOAN_DEDUCTION");
        builder.HasKey(e => e.DedId);

        builder.Property(e => e.DedId).HasColumnName("DED_ID").ValueGeneratedOnAdd();
        builder.Property(e => e.LoanNo).HasColumnName("LOAN_NO");
        builder.Property(e => e.ContributionId).HasColumnName("CONTRIBUTION_ID").HasColumnType("decimal(38)");
        builder.Property(e => e.DedAmount).HasColumnName("DED_AMOUNT").HasColumnType("decimal(19,0)");
        builder.Property(e => e.DedDate).HasColumnName("DED_DATE").HasColumnType("datetime2(3)");

        builder.Ignore(e => e.DomainEvents);
    }
}
