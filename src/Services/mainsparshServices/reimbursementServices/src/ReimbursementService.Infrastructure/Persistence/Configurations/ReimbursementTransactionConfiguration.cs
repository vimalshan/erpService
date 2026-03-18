using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;

namespace ReimbursementService.Infrastructure.Persistence.Configurations;

public sealed class ReimbursementTransactionConfiguration : IEntityTypeConfiguration<ReimbursementTransaction>
{
    public void Configure(EntityTypeBuilder<ReimbursementTransaction> builder)
    {
        builder.ToTable("REIM_TRAN");

        builder.HasKey(x => x.ReimId);

        builder.Property(x => x.ReimId)
            .HasColumnName("REIM_ID")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ReimRefNo)
            .HasColumnName("REIM_REF_NO")
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.ReimRefNo).IsUnique();

        builder.Property(x => x.EmpSysId)
            .HasColumnName("EMP_SYSID")
            .IsRequired();

        builder.Property(x => x.ReimType)
            .HasColumnName("REIM_TYPE")
            .HasMaxLength(100)
            .HasConversion(v => v.ToString().ToUpperInvariant(), v => Enum.Parse<ReimbursementType>(v, true));

        // Owned Money value object mapped to two columns
        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("REIM_AMOUNT")
                .HasColumnType("decimal(19,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("REIM_CURRENCY")
                .HasMaxLength(10)
                .HasDefaultValue("INR");
        });

        builder.Property(x => x.ReimDate)
            .HasColumnName("REIM_DATE")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.ExpenseDate)
            .HasColumnName("EXPENSE_DATE")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("DESCRIPTION")
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Location)
            .HasColumnName("LOCATION")
            .HasMaxLength(255);

        builder.Property(x => x.Status)
            .HasColumnName("REIM_STATUS")
            .HasMaxLength(20)
            .HasDefaultValueSql("'DRAFT'")
            .HasConversion(v => v.ToString().ToUpperInvariant(), v => Enum.Parse<ReimbursementStatus>(v, true))
            .HasSentinel(ReimbursementStatus.Draft);

        builder.Property(x => x.ApprovalLevel).HasColumnName("APPROVAL_LEVEL");
        builder.Property(x => x.ApprovedBy).HasColumnName("APPROVED_BY");
        builder.Property(x => x.ApprovedOn).HasColumnName("APPROVED_ON").HasColumnType("datetime2(3)");

        builder.Property(x => x.RejectionReason)
            .HasColumnName("REJECTION_REASON")
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.PaymentDate)
            .HasColumnName("PAYMENT_DATE")
            .HasColumnType("date");

        builder.Property(x => x.CreatedBy).HasColumnName("CREATED_BY").IsRequired();

        builder.Property(x => x.CreatedOn)
            .HasColumnName("CREATED_ON")
            .HasColumnType("datetime2(3)")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(x => x.UpdatedBy).HasColumnName("UPDATED_BY");
        builder.Property(x => x.UpdatedOn).HasColumnName("UPDATED_ON").HasColumnType("datetime2(3)");

        // Indexes matching the SQL schema
        builder.HasIndex(x => x.EmpSysId).HasDatabaseName("IX_REIM_TRAN_EMP_SYSID");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_REIM_TRAN_STATUS");
        builder.HasIndex(x => x.ReimType).HasDatabaseName("IX_REIM_TRAN_TYPE");
        builder.HasIndex(x => x.ReimDate).HasDatabaseName("IX_REIM_TRAN_DATE");

        // Ignore domain events — not persisted
        builder.Ignore(x => x.DomainEvents);
    }
}
