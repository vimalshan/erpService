using LoanManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Infrastructure.Data.Configurations;

public class LoanInterestConfiguration : IEntityTypeConfiguration<LoanInterest>
{
    public void Configure(EntityTypeBuilder<LoanInterest> builder)
    {
        builder.ToTable("LOAN_INTEREST");

        builder.HasKey(x => x.IntId);
        builder.Property(x => x.IntId).HasColumnName("INT_ID");
        builder.Property(x => x.IntLoanId).HasColumnName("INT_LOANID");
        builder.Property(x => x.IntRateType).HasColumnName("INT_RATETYPE").HasMaxLength(2);
        builder.Property(x => x.IntPer).HasColumnName("INT_PER").HasColumnType("decimal(19,0)");
        builder.Property(x => x.IntFloatTypeId).HasColumnName("INT_FLOATTYPEID");
        builder.Property(x => x.IntEffDate).HasColumnName("INT_EFFDATE");
        builder.Property(x => x.IntClsDate).HasColumnName("INT_CLSDATE");

        builder.HasIndex(x => x.IntLoanId).HasDatabaseName("IX_LOAN_INTEREST_LOANID");

        builder.Ignore(x => x.DomainEvents);
        builder.Ignore(x => x.IsFixed);
        builder.Ignore(x => x.IsFloating);
    }
}
