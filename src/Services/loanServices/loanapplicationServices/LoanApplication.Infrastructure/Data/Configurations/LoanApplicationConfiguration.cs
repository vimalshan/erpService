using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanApplication.Domain.Aggregates;
using LoanApplication.Domain.ValueObjects;

namespace LoanApplication.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for LoanApplicationAggregate
/// </summary>
public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplicationAggregate>
{
    public void Configure(EntityTypeBuilder<LoanApplicationAggregate> builder)
    {
        // Table mapping
        builder.ToTable("LOAN_APPLICATION");

        // Primary key
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("LOAN_APPID")
            .ValueGeneratedOnAdd();

        // Property mappings
        builder.Property(x => x.EmployeeId)
            .HasColumnName("LOAN_EMPSYSID")
            .IsRequired();

        builder.Property(x => x.LoanId)
            .HasColumnName("LOAN_ID")
            .IsRequired();

        builder.Property(x => x.AppliedBy)
            .HasColumnName("LOAN_APPLIEDBY")
            .IsRequired();

        builder.Property(x => x.AppliedOn)
            .HasColumnName("LOAN_APPLIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        // Value object: LoanSource
        builder.Property(x => x.Source)
            .HasColumnName("LOAN_SOURCE")
            .HasConversion(
                v => v.Value,
                v => LoanSource.FromValue(v))
            .HasMaxLength(3)
            .IsRequired();

        // Value object: Money
        builder.Property(x => x.Amount)
            .HasColumnName("LOAN_AMOUNT")
            .HasConversion(
                v => v.Amount,
                v => Money.Create(v))
            .IsRequired();

        builder.Property(x => x.SubclassId)
            .HasColumnName("LOAN_SUBCLASSID");

        builder.Property(x => x.Reason)
            .HasColumnName("LOAN_REASON")
            .HasMaxLength(200)
            .IsRequired();

        // Value object: LoanApplicationStatus
        builder.Property(x => x.Status)
            .HasColumnName("LOAN_APPSTATUS")
            .HasConversion(
                v => v.Value,
                v => LoanApplicationStatus.FromValue(v))
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(x => x.GuarantorId)
            .HasColumnName("LOAN_GUARANTOR")
            .IsRequired();

        builder.Property(x => x.ApprovalRemarks)
            .HasColumnName("LOAN_APRREMARKS")
            .HasMaxLength(200);

        builder.Property(x => x.RequiredBy)
            .HasColumnName("LOAN_REQUIREDBY");

        builder.Property(x => x.ApprovedBy)
            .HasColumnName("LOAN_APPROVEDBY");

        builder.Property(x => x.ApprovedOn)
            .HasColumnName("LOAN_APPROVEDON")
            .HasColumnType("datetime2(3)");

        builder.Property(x => x.ModifiedBy)
            .HasColumnName("LOAN_MODIFIEDBY")
            .IsRequired();

        builder.Property(x => x.ModifiedAt)
            .HasColumnName("LOAN_MODIFIEDON")
            .HasColumnType("datetime2(3)")
            .IsRequired();

        builder.Property(x => x.TenureMonths)
            .HasColumnName("LOAN_TENURE");

        builder.Property(x => x.SecondGuarantorId)
            .HasColumnName("LOAN_GUARANTOR2");

        builder.Property(x => x.SpecialSanction)
            .HasColumnName("LOAN_SPLSANCTION")
            .HasMaxLength(1);

        // Audit fields
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Global query filter for soft delete
        builder.HasQueryFilter(x => !x.IsDeleted);

        // Index on EmployeeId
        builder.HasIndex(x => x.EmployeeId)
            .HasDatabaseName("IDX_LOAN_APPLICATION_EMPSYSID");

        // Index on Status
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IDX_LOAN_APPLICATION_STATUS");
    }
}
