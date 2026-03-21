namespace TransactionService.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Entities;
using TransactionService.Domain.ValueObjects;

public sealed class UnitBudgetConfiguration : IEntityTypeConfiguration<UnitBudget>
{
    public void Configure(EntityTypeBuilder<UnitBudget> builder)
    {
        builder.ToTable("SP_UNIT_BUDGET");
        builder.HasKey(b => new { b.LocationId, b.FinYearId });
        builder.Property(b => b.LocationId).HasColumnName("UB_LOCATION_ID");
        builder.Property(b => b.UnitCode)
            .HasColumnName("UB_UNIT_CODE")
            .HasMaxLength(3)
            .HasConversion(u => u.Value, v => new UnitCode(v));
        builder.Property(b => b.FinYearId).HasColumnName("UB_FINYEAR_ID");
        builder.Property(b => b.BudgetAmount)
            .HasColumnName("UB_BUDGETAMOUNT")
            .HasConversion(m => m.Amount, v => new Money(v));
        builder.Property(b => b.UpdatedBy).HasColumnName("UB_UPDATED_BY");
        builder.Property(b => b.UpdatedOn).HasColumnName("UB_UPDATED_ON");

        builder.Ignore(b => b.DomainEvents);
    }
}
