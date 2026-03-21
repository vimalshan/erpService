using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class TravelExpenseAllocation : BaseEntity
{
    public long? RequestNumber { get; set; }
    public long? AllocationSerialNumber { get; set; }
    public long? ExpenseSerialNumber { get; set; }
    public string? UnitCode { get; set; }
    public string? CostCentreCode { get; set; }
    public string? AllocationType { get; set; }
    public decimal? AllocationPercentage { get; set; }

    // Navigation
    public TravelExpense? TravelExpense { get; set; }
}
