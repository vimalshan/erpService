namespace ExpenseService.Application.DTOs;

public record ExpenseSummaryDto
{
    public long? TravelPlanNo { get; init; }
    public decimal? BudgetAmount { get; init; }
    public decimal TotalExpenses { get; init; }
    public decimal TotalVariance { get; init; }
    public decimal EmployeeShare { get; init; }
    public decimal CompanyShare { get; init; }
    public string? Status { get; init; }
    public decimal TotalDAAmount { get; init; }
}
