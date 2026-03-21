namespace ExpenseService.Application.DTOs;

public record TravelExpenseDto
{
    public long RequestNumber { get; init; }
    public long SerialNumber { get; init; }
    public long? ExpenseCode { get; init; }
    public string? CurrencyType { get; init; }
    public long? EligibleAmount { get; init; }
    public decimal? BudgetAmount { get; init; }
    public string? CompanyExpense { get; init; }
    public decimal? SelfExpense { get; init; }
    public decimal? VarianceAmount { get; init; }
    public string? ExpenseRemarks { get; init; }
    public long? TransactionNumber { get; init; }
    public decimal? ExpenseAnnexure { get; init; }
    public List<ExpenseAllocationDto> Allocations { get; init; } = [];
    public List<ExpenseSubDetailDto> SubDetails { get; init; } = [];
}
