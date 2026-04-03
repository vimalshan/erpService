using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanTransaction.Domain.Aggregates;
using LoanTransaction.Domain.ValueObjects;

namespace LoanTransaction.Infrastructure.Data.Configurations;

public class LoanMainConfiguration : IEntityTypeConfiguration<LoanAggregate>
{
    public void Configure(EntityTypeBuilder<LoanAggregate> b)
    {
        b.ToTable("LOAN_MAIN");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("LOAN_NO").ValueGeneratedOnAdd();

        b.Property(x => x.ApplicationId).HasColumnName("LOAN_APPID").IsRequired();
        b.Property(x => x.EmployeeId).HasColumnName("LOAN_EMPSYSID").IsRequired();
        b.Property(x => x.LoanDefinitionId).HasColumnName("LOAN_ID").IsRequired();
        b.Property(x => x.GradeId).HasColumnName("LOAN_GRADEID").IsRequired();
        b.Property(x => x.UnitId).HasColumnName("LOAN_UNITID").IsRequired();
        b.Property(x => x.SubclassId).HasColumnName("LOAN_SUBCLASSID").IsRequired();
        b.Property(x => x.GuarantorId).HasColumnName("LOAN_GUARANTOR").IsRequired();

        b.Property(x => x.DisbursementType)
            .HasColumnName("LOAN_DISBTYPE")
            .HasConversion(v => v.Value, v => DisbursementType.FromValue(v))
            .HasMaxLength(3).IsRequired();

        b.Property(x => x.PrincipalAmount)
            .HasColumnName("LOAN_PRNAMT")
            .HasConversion(v => v.Amount, v => Money.Create(v))
            .HasColumnType("decimal(19,0)").IsRequired();

        b.Property(x => x.OldPrincipalAdj)
            .HasColumnName("LOAN_OLDPRNADJ")
            .HasConversion(v => v.Amount, v => Money.Create(v))
            .HasColumnType("decimal(19,0)").IsRequired();

        b.Property(x => x.AmountPaid)
            .HasColumnName("LOAN_PAID")
            .HasConversion(v => v.Amount, v => Money.Create(v))
            .HasColumnType("decimal(19,0)").IsRequired();

        b.Property(x => x.PrincipalOutstanding)
            .HasColumnName("LOAN_PRNOUT")
            .HasConversion(v => v.Amount, v => Money.Create(v))
            .HasColumnType("decimal(19,0)").IsRequired();

        b.Property(x => x.EffectiveDate).HasColumnName("LOAN_DATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.FirstInstallmentDate).HasColumnName("LOAN_FIRSTINSDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.LastInstallmentDate).HasColumnName("LOAN_LASTINSDATE").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.ClosureDate).HasColumnName("LOAN_CLSDATE").HasColumnType("datetime2(3)");

        b.Property(x => x.Reason).HasColumnName("LOAN_REASON").HasMaxLength(200).IsRequired();
        b.Property(x => x.ApprovalRemarks).HasColumnName("LOAN_APRREMARKS").HasMaxLength(200);

        b.Property(x => x.ClosureType)
            .HasColumnName("LOAN_CLOSURETYPE")
            .HasConversion(v => v.Value, v => ClosureType.FromValue(v))
            .HasMaxLength(3).IsRequired();

        b.Property(x => x.NewLoanNo).HasColumnName("LOAN_NEWLOANNO").IsRequired();
        b.Property(x => x.HasEmployeeInterestRate)
            .HasColumnName("LOAN_EMPINTRATE")
            .HasConversion(v => v ? 'Y' : 'N', v => v == 'Y')
            .HasMaxLength(1).IsRequired();
        b.Property(x => x.CompoundingFactor).HasColumnName("LOAN_COMFACTOR").HasMaxLength(1).IsRequired();
        b.Property(x => x.InterestFrequency).HasColumnName("LOAN_INTFREQUENCY").HasMaxLength(1).IsRequired();

        b.Property(x => x.RecoveryMethod)
            .HasColumnName("LOAN_RECTYPE")
            .HasConversion(v => v.Value, v => RecoveryMethod.FromValue(v))
            .HasMaxLength(3).IsRequired();

        b.Property(x => x.CreatedBy).HasColumnName("LOAN_CREATEDBY").IsRequired();
        b.Property(x => x.CreatedAt).HasColumnName("LOAN_CREATEDON").HasColumnType("datetime2(3)").IsRequired();
        b.Property(x => x.ModifiedBy).HasColumnName("LOAN_MODIFIEDBY").IsRequired();
        b.Property(x => x.ModifiedAt).HasColumnName("LOAN_MODIFIEDON").HasColumnType("datetime2(3)").IsRequired();

        b.Property(x => x.AmountEdId).HasColumnName("LOAN_AMTEDID").IsRequired();
        b.Property(x => x.PrnEdId).HasColumnName("LOAN_PRNEDID").IsRequired();
        b.Property(x => x.IntEdId).HasColumnName("LOAN_INTEDID").IsRequired();
        b.Property(x => x.EmpInstallmentNos).HasColumnName("LOAN_EMPINSNOS");
        b.Property(x => x.EmpInstallmentAmount)
            .HasColumnName("LOAN_EMPINSAMT")
            .HasConversion(v => v == null ? (decimal?)null : v.Amount, v => v == null ? null : Money.Create(v.Value))
            .HasColumnType("decimal(19,0)");

        b.Ignore(x => x.IsDeleted);
        b.Ignore(x => x.Installments);
        b.Ignore(x => x.Settlements);
        b.Ignore(x => x.LedgerEntries);
        b.Ignore(x => x.InterestRates);
        b.Ignore(x => x.Adjustments);

        b.HasIndex(x => x.EmployeeId).HasDatabaseName("IDX_LOAN_MAIN_EMPSYSID");
        b.HasIndex(x => x.ApplicationId).HasDatabaseName("IDX_LOAN_MAIN_APPID");
        b.HasIndex(x => x.ClosureDate).HasDatabaseName("IDX_LOAN_MAIN_CLSDATE");
    }
}
