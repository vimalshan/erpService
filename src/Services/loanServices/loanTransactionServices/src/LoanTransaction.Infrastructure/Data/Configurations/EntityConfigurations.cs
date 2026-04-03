using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanTransaction.Domain.Entities;

namespace LoanTransaction.Infrastructure.Data.Configurations;

public class LoanInstallmentConfiguration : IEntityTypeConfiguration<LoanInstallment>
{
    public void Configure(EntityTypeBuilder<LoanInstallment> b)
    {
        b.ToTable("LOAN_INS");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOANINS_ID").ValueGeneratedOnAdd();
        b.Property(x => x.LoanNo).HasColumnName("LOANINS_LOANNO").IsRequired();
        b.Property(x => x.UnitId).HasColumnName("LOANINS_UNITID").IsRequired();
        b.Property(x => x.InstallmentDate).HasColumnName("LOANINS_INSDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.InstallmentNo).HasColumnName("LOANINS_INSNO").IsRequired();
        b.Property(x => x.InstallmentAmount).HasColumnName("LOANINS_INSAMT").IsRequired();
        b.Property(x => x.PrincipalOutstanding).HasColumnName("LOANINS_PRNOUT").IsRequired();
        b.Property(x => x.PrincipalAdjusted).HasColumnName("LOANINS_PRNADJ").IsRequired();
        b.Property(x => x.InterestAdjusted).HasColumnName("LOANINS_INTADJ").IsRequired();
        b.Property(x => x.FromDate).HasColumnName("LOANINS_FRODATE").HasColumnType("datetime2(3)");
        b.Property(x => x.InterestAccrued).HasColumnName("LOANINS_INTACC").IsRequired();
        b.Property(x => x.InterestRecovered).HasColumnName("LOANINS_INTREC").IsRequired();
        b.Property(x => x.PrincipalRecovered).HasColumnName("LOANINS_PRNREC").IsRequired();
        b.Property(x => x.InterestRate).HasColumnName("LOANINS_INTRATE").IsRequired();
        b.Property(x => x.Remarks).HasColumnName("LOANINS_REMARKS").HasMaxLength(200).IsRequired();
        b.Property(x => x.UpdatedBy).HasColumnName("LOANINS_UPDATEDBY").IsRequired();
        b.Property(x => x.UpdatedOn).HasColumnName("LOANINS_UPDATEDON").HasColumnType("datetime2(3)").IsRequired();
        b.HasIndex(x => x.LoanNo).HasDatabaseName("IDX_LOAN_INS_LOANNO");
        b.HasIndex(new[] { nameof(LoanInstallment.LoanNo), nameof(LoanInstallment.InstallmentNo) });
    }
}

public class LoanSettlementConfiguration : IEntityTypeConfiguration<LoanSettlement>
{
    public void Configure(EntityTypeBuilder<LoanSettlement> b)
    {
        b.ToTable("LOAN_SET");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOANSET_ID").ValueGeneratedOnAdd();
        b.Property(x => x.UnitId).HasColumnName("LOANSET_UNITID").IsRequired();
        b.Property(x => x.LoanNo).HasColumnName("LOANSET_LOANNO").IsRequired();
        b.Property(x => x.SettlementType).HasColumnName("LOANSET_TYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.InstallmentNo).HasColumnName("LOANSET_INSNO").IsRequired();
        b.Property(x => x.InstallmentDate).HasColumnName("LOANSET_INSDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.RecoveryDate).HasColumnName("LOANSET_RECDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.RecoveryType).HasColumnName("LOANSET_RECTYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.InstallmentAmount).HasColumnName("LOANSET_INSAMT").IsRequired();
        b.Property(x => x.PayType).HasColumnName("LOANSET_PAYTYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.PayBatchId).HasColumnName("LOANSET_PAYBATCHID").IsRequired();
        b.Property(x => x.PayId).HasColumnName("LOANSET_PAYID").IsRequired();
        b.Property(x => x.AdjustLoanNo).HasColumnName("LOANSET_ADJLOANNO").IsRequired();
        b.Property(x => x.CancelDate).HasColumnName("LOANSET_CANCELDATE").HasColumnType("datetime2(3)");
        b.Property(x => x.CancelBy).HasColumnName("LOANSET_CANCELBY");
        b.Property(x => x.UpdatedBy).HasColumnName("LOANSET_UPDATEDBY").IsRequired();
        b.Property(x => x.UpdatedOn).HasColumnName("LOANSET_UPDATEDON").HasColumnType("datetime2(3)").IsRequired();
        b.HasIndex(x => x.LoanNo).HasDatabaseName("IDX_LOAN_SET_LOANNO");
    }
}

public class LoanLedgerConfiguration : IEntityTypeConfiguration<LoanLedger>
{
    public void Configure(EntityTypeBuilder<LoanLedger> b)
    {
        b.ToTable("LOAN_LEDGER");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOAN_LEDGERID").ValueGeneratedOnAdd();
        b.Property(x => x.LoanNo).HasColumnName("LOAN_NO").IsRequired();
        b.Property(x => x.EmployeeId).HasColumnName("LOAN_EMPSYSID").IsRequired();
        b.Property(x => x.UnitId).HasColumnName("LOAN_UNITID").IsRequired();
        b.Property(x => x.EmployeeNo).HasColumnName("LOAN_EMPNO").IsRequired();
        b.Property(x => x.TransactionDate).HasColumnName("LOAN_TRNDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.DCFlag).HasColumnName("LOAN_DCFLAG").HasMaxLength(1).IsRequired();
        b.Property(x => x.Description).HasColumnName("LOAN_DESCRIPTION").HasMaxLength(200).IsRequired();
        b.Property(x => x.TransactionAmount).HasColumnName("LOAN_TRNAMT").IsRequired();
        b.Property(x => x.TransactionType).HasColumnName("LOAN_TRNTYPE").HasMaxLength(3).IsRequired();
        b.Property(x => x.TransactionRefNo).HasColumnName("LOAN_TRNREFNUM").IsRequired();
        b.Property(x => x.ScheduleId).HasColumnName("LOAN_SCHEDULEID").IsRequired();
        b.Property(x => x.UpdatedBy).HasColumnName("LOAN_UPDATEDBY").IsRequired();
        b.Property(x => x.UpdatedOn).HasColumnName("LOAN_UPDATEDON").HasColumnType("datetime2(3)").IsRequired();
        b.HasIndex(x => x.LoanNo).HasDatabaseName("IDX_LOAN_LEDGER_NO");
        b.HasIndex(x => x.EmployeeId).HasDatabaseName("IDX_LOAN_LEDGER_EMPSYSID");
    }
}

public class LoanEmpInterestRateConfiguration : IEntityTypeConfiguration<LoanEmpInterestRate>
{
    public void Configure(EntityTypeBuilder<LoanEmpInterestRate> b)
    {
        b.ToTable("LOAN_EMPINTRATEMAST");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOANINT_RATEID").ValueGeneratedOnAdd();
        b.Property(x => x.LoanNo).HasColumnName("LOANINT_LOANNO").IsRequired();
        b.Property(x => x.EffectiveDate).HasColumnName("LOANINT_EFFDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.ClosureDate).HasColumnName("LOANINT_CLSDATE").HasColumnType("datetime2(3)");
        b.Property(x => x.Rate).HasColumnName("LOANINT_RATE").IsRequired();
        b.Property(x => x.EmiAmount).HasColumnName("LOANINT_EMIAMT").IsRequired();
        b.Property(x => x.NumberOfInstallments).HasColumnName("LOANINT_INSNOS").IsRequired();
        b.Property(x => x.LastModifiedBy).HasColumnName("LOANINT_LASTMODIFIEDBY").IsRequired();
        b.Property(x => x.LastModifiedOn).HasColumnName("LOANINT_LASTMODIFIEDON").HasColumnType("datetime2(3)").IsRequired();
        b.HasIndex(x => x.LoanNo).HasDatabaseName("IDX_LOAN_EMPINTRATEMAST_LOANNO");
    }
}

public class LoanAdjustmentConfiguration : IEntityTypeConfiguration<LoanAdjustment>
{
    public void Configure(EntityTypeBuilder<LoanAdjustment> b)
    {
        b.ToTable("LOAN_ADJUSTMENT");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOAN_ADJID").ValueGeneratedOnAdd();
        b.Property(x => x.LoanNo).HasColumnName("LOAN_NO").IsRequired();
        b.Property(x => x.AdjLoanNo).HasColumnName("LOAN_ADJLOANNO").IsRequired();
        b.Property(x => x.AdjPrincipalAmount).HasColumnName("LOAN_ADJPRNAMT").IsRequired();
        b.Property(x => x.AdjInterestAmount).HasColumnName("LOAN_ADJINTAMT").IsRequired();
        b.Property(x => x.UpdatedBy).HasColumnName("LOAN_UPDATEDBY").IsRequired();
        b.Property(x => x.UpdatedOn).HasColumnName("LOAN_UPDATEDON").HasColumnType("datetime2(3)").IsRequired();
        b.HasIndex(x => x.LoanNo).HasDatabaseName("IDX_LOAN_ADJUSTMENT_NO");
    }
}
