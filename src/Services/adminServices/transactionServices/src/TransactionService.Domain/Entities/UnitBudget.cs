namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.ValueObjects;

public sealed class UnitBudget : Entity
{
    public long LocationId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public long FinYearId { get; private set; }
    public Money BudgetAmount { get; private set; } = Money.Zero;
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private UnitBudget() { }

    public static UnitBudget Create(
        long locationId, string unitCode, long finYearId,
        long budgetAmount, long updatedBy)
    {
        return new UnitBudget
        {
            LocationId = locationId,
            UnitCode = new UnitCode(unitCode),
            FinYearId = finYearId,
            BudgetAmount = new Money(budgetAmount),
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void UpdateBudget(long newAmount, long updatedBy)
    {
        BudgetAmount = new Money(newAmount);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
