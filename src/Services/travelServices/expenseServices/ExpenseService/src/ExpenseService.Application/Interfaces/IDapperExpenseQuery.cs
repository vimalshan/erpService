using ExpenseService.Application.DTOs;

namespace ExpenseService.Application.Interfaces;

public interface IDapperExpenseQuery
{
    Task<ExpenseSummaryDto?> GetExpenseSummaryAsync(long requestNumber);
    Task<decimal?> GetDARateAsync(string gradeCode, string arrangementType, DateTime fromDate, DateTime toDate);
    Task<SettlementResultDto> SettleExpensesAsync(long requestNumber);
}
