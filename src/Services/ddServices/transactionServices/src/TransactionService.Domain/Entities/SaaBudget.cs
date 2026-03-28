namespace TransactionService.Domain.Entities;

public class SaaBudget : BaseEntity
{
    public long BusinessId { get; set; }
    public long YearId { get; set; }
    public decimal BudgetAmount { get; set; }
    public long UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

    public SaaBudget() { }

    public SaaBudget(long businessId, long yearId, decimal budgetAmount, long updatedBy)
    {
        BusinessId = businessId;
        YearId = yearId;
        BudgetAmount = budgetAmount;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
