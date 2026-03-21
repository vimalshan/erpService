namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.ValueObjects;

public sealed class DeptBudget : Entity
{
    public long LocationId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public long DeptId { get; private set; }
    public long FinYearId { get; private set; }
    public Money BudgetAmount { get; private set; } = Money.Zero;
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private DeptBudget() { }

    public static DeptBudget Create(
        long locationId, string unitCode, long deptId,
        long finYearId, long budgetAmount, long updatedBy)
    {
        return new DeptBudget
        {
            LocationId = locationId,
            UnitCode = new UnitCode(unitCode),
            DeptId = deptId,
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
