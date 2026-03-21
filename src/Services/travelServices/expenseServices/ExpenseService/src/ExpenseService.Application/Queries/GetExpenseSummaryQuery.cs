using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetExpenseSummaryQuery : IRequest<ExpenseSummaryDto?>
{
    public long RequestNumber { get; init; }
}
