using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Commands;

public record RecordExpenseCommand : IRequest<TravelExpenseDto>
{
    public long RequestNumber { get; init; }
    public long ExpenseCode { get; init; }
    public string? CurrencyType { get; init; }
    public decimal BudgetAmount { get; init; }
    public long EligibleAmount { get; init; }
    public decimal CompanyAmount { get; init; }
    public decimal SelfAmount { get; init; }
    public string? ExpenseRemarks { get; init; }
}
