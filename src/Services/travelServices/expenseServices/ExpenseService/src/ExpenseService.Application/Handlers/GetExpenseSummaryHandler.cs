using ExpenseService.Application.DTOs;
using ExpenseService.Application.Interfaces;
using ExpenseService.Application.Queries;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class GetExpenseSummaryHandler : IRequestHandler<GetExpenseSummaryQuery, ExpenseSummaryDto?>
{
    private readonly IDapperExpenseQuery _dapperQuery;

    public GetExpenseSummaryHandler(IDapperExpenseQuery dapperQuery)
    {
        _dapperQuery = dapperQuery;
    }

    public async Task<ExpenseSummaryDto?> Handle(GetExpenseSummaryQuery request, CancellationToken cancellationToken)
    {
        return await _dapperQuery.GetExpenseSummaryAsync(request.RequestNumber);
    }
}
