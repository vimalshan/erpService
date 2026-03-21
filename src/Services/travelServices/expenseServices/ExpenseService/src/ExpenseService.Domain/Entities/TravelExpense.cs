using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class TravelExpense : BaseEntity
{
    public long RequestNumber { get; set; }
    public long SerialNumber { get; set; }
    public long? ExpenseCode { get; set; }
    public string? CurrencyType { get; set; }
    public long? EligibleAmount { get; set; }
    public decimal? BudgetAmount { get; set; }
    public string? CompanyExpense { get; set; }
    public decimal? SelfExpense { get; set; }
    public decimal? VarianceAmount { get; set; }
    public string? ExpenseRemarks { get; set; }
    public long? TransactionNumber { get; set; }
    public decimal? ExpenseAnnexure { get; set; }

    // Navigation properties
    public ICollection<TravelExpenseAllocation> Allocations { get; set; } = [];
    public ICollection<TravelExpenseSub> SubDetails { get; set; } = [];
}
