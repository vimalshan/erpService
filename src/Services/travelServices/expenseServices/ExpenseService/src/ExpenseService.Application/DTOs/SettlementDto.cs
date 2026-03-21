namespace ExpenseService.Application.DTOs;

public record SettlementDto
{
    public long? ExpenseCode { get; init; }
    public string? ExpenseName { get; init; }
    public decimal? BudgetAmount { get; init; }
    public decimal? CompanyAmount { get; init; }
    public decimal? SelfAmount { get; init; }
    public decimal? AnnexureAmount { get; init; }
    public string? Remarks { get; init; }
}
