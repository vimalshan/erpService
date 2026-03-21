using ExpenseService.Application.Commands;
using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.API.GraphQL;

public class ExpenseMutation
{
    public async Task<TravelExpenseDto> RecordExpense(
        [Service] IMediator mediator,
        RecordExpenseCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<DaSummaryDto> CalculateDA(
        [Service] IMediator mediator,
        CalculateDACommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<SettlementResultDto> SettleExpenses(
        [Service] IMediator mediator,
        long requestNumber)
    {
        return await mediator.Send(new SettleExpensesCommand { RequestNumber = requestNumber });
    }

    public async Task<ConveyanceDto> CreateConveyance(
        [Service] IMediator mediator,
        CreateConveyanceCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<CurrencyDto> CreateCurrencyRequest(
        [Service] IMediator mediator,
        CreateCurrencyRequestCommand input)
    {
        return await mediator.Send(input);
    }

    public async Task<bool> DeleteExpense(
        [Service] IMediator mediator,
        long requestNumber,
        long serialNumber)
    {
        return await mediator.Send(new DeleteExpenseCommand
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber
        });
    }
}
