using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanAccount.Domain.Entities;
using LoanAccount.Domain.ValueObjects;

namespace LoanAccount.Infrastructure.Configuration;

/// <summary>
/// EF Core configuration for LoanMain entity
/// </summary>
public class LoanMainConfiguration : IEntityTypeConfiguration<LoanMain>
{
    public void Configure(EntityTypeBuilder<LoanMain> builder)
    {
        builder.HasKey(l => l.Id);
        
        // ID is not auto-generated, it's set explicitly to LoanNo
        builder.Property(l => l.Id)
            .ValueGeneratedNever();

        builder.Property(l => l.LoanNo)
            .IsRequired();

        builder.Property(l => l.LoanAppId)
            .IsRequired();

        builder.Property(l => l.EmpSysId)
            .IsRequired();

        builder.Property(l => l.GradeId)
            .IsRequired();

        builder.Property(l => l.LoanDate)
            .IsRequired();

        builder.Property(l => l.FirstInstallmentDate)
            .IsRequired();

        builder.Property(l => l.Reason)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(l => l.ApprovalRemarks)
            .HasMaxLength(200);

        // Configure value objects
        builder.Property(l => l.PrincipalAmount)
            .HasConversion(
                v => v.Amount,
                v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(l => l.DisbursedAmount)
            .HasConversion(
                v => v.Amount,
                v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(l => l.PrincipalOutstanding)
            .HasConversion(
                v => v.Amount,
                v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(l => l.OldPrincipalAdjustment)
            .HasConversion(
                v => v.Amount,
                v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(l => l.EmployeeSpecificInstallmentAmount)
            .HasConversion(
                v => v != null ? v.Amount : (decimal?)null,
                v => v != null ? new Money(v.Value) : null)
            .HasPrecision(19, 2);

        builder.Property(l => l.DisbursementType)
            .HasConversion(v => v.Type, v => DisbursementType.Create(v));

        builder.Property(l => l.LoanStatus)
            .HasConversion(v => v.Status, v => LoanStatus.Create(v));

        builder.Property(l => l.RecoveryMethod)
            .HasConversion(v => v.Method, v => RecoveryMethod.Create(v));

        builder.HasIndex(l => l.LoanNo).IsUnique();
        builder.HasIndex(l => l.LoanAppId);
        builder.HasIndex(l => l.EmpSysId);
        builder.HasIndex(l => l.UnitId);
    }
}

/// <summary>
/// EF Core configuration for LoanInstallment entity
/// </summary>
public class LoanInstallmentConfiguration : IEntityTypeConfiguration<LoanInstallment>
{
    public void Configure(EntityTypeBuilder<LoanInstallment> builder)
    {
        builder.HasKey(li => li.Id);
        
        // ID is not auto-generated, it's set explicitly
        builder.Property(li => li.Id)
            .ValueGeneratedNever();

        builder.Property(li => li.LoanNo)
            .IsRequired();

        builder.Property(li => li.InstallmentNo)
            .IsRequired();

        builder.Property(li => li.InstallmentDate)
            .IsRequired();

        builder.Property(li => li.Remarks)
            .HasMaxLength(200);

        // Configure value objects
        builder.Property(li => li.InstallmentAmount)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(li => li.PrincipalOutstanding)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(li => li.PrincipalAdjustment)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(li => li.InterestAdjustment)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(li => li.InterestAccrued)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(li => li.InterestRecovered)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(li => li.PrincipalRecovered)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2);

        builder.Property(li => li.InterestRate)
            .HasConversion(v => v.Rate, v => new InterestRate(v))
            .IsRequired();

        builder.HasIndex(li => li.LoanNo);
        builder.HasIndex(li => new { li.LoanNo, li.InstallmentNo });
    }
}

/// <summary>
/// EF Core configuration for LoanEmployeeInterestRate entity
/// </summary>
public class LoanEmployeeInterestRateConfiguration : IEntityTypeConfiguration<LoanEmployeeInterestRate>
{
    public void Configure(EntityTypeBuilder<LoanEmployeeInterestRate> builder)
    {
        builder.HasKey(leir => leir.Id);
        
        // ID is not auto-generated, it's set explicitly
        builder.Property(leir => leir.Id)
            .ValueGeneratedNever();

        builder.Property(leir => leir.LoanNo)
            .IsRequired();

        builder.Property(leir => leir.InstallmentNumbers)
            .IsRequired();

        builder.Property(leir => leir.EffectiveDate)
            .IsRequired();

        // Configure value objects
        builder.Property(leir => leir.InterestRate)
            .HasConversion(v => v.Rate, v => new InterestRate(v))
            .IsRequired();

        builder.Property(leir => leir.EMIAmount)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.HasIndex(leir => leir.LoanNo);
    }
}

/// <summary>
/// EF Core configuration for LoanLedger entity
/// </summary>
public class LoanLedgerConfiguration : IEntityTypeConfiguration<LoanLedger>
{
    public void Configure(EntityTypeBuilder<LoanLedger> builder)
    {
        builder.HasKey(ll => ll.Id);
        
        // ID is not auto-generated, it's set explicitly
        builder.Property(ll => ll.Id)
            .ValueGeneratedNever();

        builder.Property(ll => ll.LoanNo)
            .IsRequired();

        builder.Property(ll => ll.EmpSysId)
            .IsRequired();

        builder.Property(ll => ll.EmpNo)
            .IsRequired();

        builder.Property(ll => ll.TransactionDate)
            .IsRequired();

        builder.Property(ll => ll.DCFlag)
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(ll => ll.Description)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(ll => ll.TransactionType)
            .HasMaxLength(10)
            .IsRequired();

        // Configure value objects
        builder.Property(ll => ll.TransactionAmount)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.HasIndex(ll => ll.LoanNo);
        builder.HasIndex(ll => ll.EmpSysId);
        builder.HasIndex(ll => ll.TransactionDate);
    }
}

/// <summary>
/// EF Core configuration for LoanSettlement entity
/// </summary>
public class LoanSettlementConfiguration : IEntityTypeConfiguration<LoanSettlement>
{
    public void Configure(EntityTypeBuilder<LoanSettlement> builder)
    {
        builder.HasKey(ls => ls.Id);
        
        // ID is not auto-generated, it's set explicitly
        builder.Property(ls => ls.Id)
            .ValueGeneratedNever();

        builder.Property(ls => ls.LoanNo)
            .IsRequired();

        builder.Property(ls => ls.InstallmentNo)
            .IsRequired();

        builder.Property(ls => ls.InstallmentDate)
            .IsRequired();

        builder.Property(ls => ls.RecoveryDate)
            .IsRequired();

        builder.Property(ls => ls.RecoveryType)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(ls => ls.PaymentType)
            .HasMaxLength(10)
            .IsRequired();

        // Configure value objects
        builder.Property(ls => ls.InstallmentAmount)
            .HasConversion(v => v.Amount, v => new Money(v))
            .HasPrecision(19, 2)
            .IsRequired();

        builder.Property(ls => ls.SettlementType)
            .HasConversion(v => v.Type, v => SettlementType.Create(v))
            .IsRequired();

        builder.HasIndex(ls => ls.LoanNo);
        builder.HasIndex(ls => ls.RecoveryDate);
    }
}
