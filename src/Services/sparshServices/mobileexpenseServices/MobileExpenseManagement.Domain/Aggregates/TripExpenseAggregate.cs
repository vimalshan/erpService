namespace MobileExpenseManagement.Domain.Aggregates;

using MobileExpenseManagement.Domain.Entities;

/// <summary>
/// Aggregate root for trip/project expenses
/// </summary>
public class TripExpenseAggregate
{
    public decimal TripId { get; private set; }
    public decimal ProjectId { get; private set; }
    public DateTime TripStartDate { get; private set; }
    public DateTime TripEndDate { get; private set; }
    public decimal TotalExpenseAmount { get; private set; }
    public int ExpenseCount { get; private set; }
    public bool IsApproved { get; private set; }
    
    // Navigation property
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public static TripExpenseAggregate Create(decimal tripId, decimal projectId, 
        DateTime tripStartDate, DateTime tripEndDate)
    {
        if (tripStartDate > tripEndDate)
            throw new ArgumentException("Trip start date cannot be after end date");

        return new TripExpenseAggregate
        {
            TripId = tripId,
            ProjectId = projectId,
            TripStartDate = tripStartDate,
            TripEndDate = tripEndDate,
            TotalExpenseAmount = 0,
            ExpenseCount = 0,
            IsApproved = false
        };
    }

    public void AddExpense(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));

        if (expense.TripId != TripId)
            throw new InvalidOperationException("Expense does not belong to this trip");

        Expenses.Add(expense);
        RecalculateTotals();
    }

    public void RemoveExpense(Expense expense)
    {
        if (expense == null)
            throw new ArgumentNullException(nameof(expense));

        Expenses.Remove(expense);
        RecalculateTotals();
    }

    public void Approve()
    {
        IsApproved = true;
    }

    private void RecalculateTotals()
    {
        TotalExpenseAmount = Expenses.Where(e => !e.IsDeleted).Sum(e => e.Amount);
        ExpenseCount = Expenses.Where(e => !e.IsDeleted).Count();
    }
}
