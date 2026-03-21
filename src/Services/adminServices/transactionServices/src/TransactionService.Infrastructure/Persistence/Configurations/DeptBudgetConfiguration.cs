namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class DeptBudgetConfiguration : IEntityTypeConfiguration<DeptBudget>
{
    public void Configure(EntityTypeBuilder<DeptBudget> builder)
    {
        builder.ToTable("SP_DEPT_BUDGET");
        builder.HasKey(b => new { b.LocationId, b.DeptId, b.FinYearId });
        builder.Property(b => b.LocationId).HasColumnName("DB_LOCATION_ID");
        builder.Property(b => b.UnitCode)
            .HasColumnName("DB_UNIT_CODE")
            .HasMaxLength(3)
            .HasConversion(u => u.Value, v => new UnitCode(v));
        builder.Property(b => b.DeptId).HasColumnName("DB_DEPT_ID");
        builder.Property(b => b.FinYearId).HasColumnName("DB_FINYEAR_ID");
        builder.Property(b => b.BudgetAmount)
            .HasColumnName("DB_BUDGETAMOUNT")
            .HasConversion(m => m.Amount, v => new Money(v));
        builder.Property(b => b.UpdatedBy).HasColumnName("DB_UPDATED_BY");
        builder.Property(b => b.UpdatedOn).HasColumnName("DB_UPDATED_ON");

        builder.Ignore(b => b.DomainEvents);
    }
}
