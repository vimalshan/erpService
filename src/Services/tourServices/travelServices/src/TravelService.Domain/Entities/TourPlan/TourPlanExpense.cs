using TravelService.Domain.Common;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlanExpense : Entity<string>
{
    public string TourPlanId { get; private set; } = string.Empty;
    public string ExpenseId { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public decimal ExpenseAmount { get; private set; }
    public string? Remarks { get; private set; }

    protected TourPlanExpense() { }

    public static TourPlanExpense Create(
        string id, string tourPlanId, string expenseId,
        string currency, decimal expenseAmount, string? remarks = null)
        => new()
        {
            Id = id,
            TourPlanId = tourPlanId,
            ExpenseId = expenseId,
            Currency = currency,
            ExpenseAmount = expenseAmount,
            Remarks = remarks
        };
}
