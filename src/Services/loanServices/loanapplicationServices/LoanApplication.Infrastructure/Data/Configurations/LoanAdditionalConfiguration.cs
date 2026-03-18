using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LoanApplication.Domain.Entities;

namespace LoanApplication.Infrastructure.Data.Configurations;

/// <summary>
/// Entity type configuration for LoanAdditional
/// </summary>
public class LoanAdditionalConfiguration : IEntityTypeConfiguration<LoanAdditional>
{
    public void Configure(EntityTypeBuilder<LoanAdditional> builder)
    {
        // Table mapping
        builder.ToTable("LOAN_ADDITIONAL");

        // Primary key - composite key
        builder.HasKey(x => new { x.EmployeeId, x.AdditionalLoanNumber });

        // Property mappings
        builder.Property(x => x.EmployeeId)
            .HasColumnName("LOAN_EMPSYSID")
            .IsRequired();

        builder.Property(x => x.AdditionalLoanNumber)
            .HasColumnName("ADDL_LOANNO")
            .IsRequired();

        builder.Property(x => x.LoanId)
            .HasColumnName("ADDL_LOANID")
            .IsRequired();

        // Audit fields
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.ModifiedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.ModifiedBy)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Global query filter for soft delete
        builder.HasQueryFilter(x => !x.IsDeleted);

        // Index on EmployeeId
        builder.HasIndex(x => x.EmployeeId)
            .HasDatabaseName("IDX_LOAN_ADDITIONAL_EMPSYSID");
    }
}
