using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Events;
using MediatR;

namespace ExpenseService.Application.Handlers;

public class SettleExpensesHandler : IRequestHandler<SettleExpensesCommand, SettlementResultDto>
{
    private readonly IDapperExpenseQuery _dapperQuery;
    private readonly IMediator _mediator;

    public SettleExpensesHandler(IDapperExpenseQuery dapperQuery, IMediator mediator)
    {
        _dapperQuery = dapperQuery;
        _mediator = mediator;
    }

    public async Task<SettlementResultDto> Handle(SettleExpensesCommand request, CancellationToken cancellationToken)
    {
        var result = await _dapperQuery.SettleExpensesAsync(request.RequestNumber);

        await _mediator.Publish(new ExpenseSettledEvent(
            request.RequestNumber, result.SettlementAmount, result.RefundAmount), cancellationToken);

        return result;
    }
}
