using ExpenseService.Application.DTOs;
using ExpenseService.Application.Queries;
using MediatR;

namespace ExpenseService.API.GraphQL;

public class ExpenseQuery
{
    public async Task<TravelExpenseDto?> GetExpense(
        [Service] IMediator mediator,
        long requestNumber,
        long serialNumber)
    {
        return await mediator.Send(new GetExpenseByIdQuery
        {
            RequestNumber = requestNumber,
            SerialNumber = serialNumber
        });
    }

    public async Task<IReadOnlyList<TravelExpenseDto>> GetExpensesByRequest(
        [Service] IMediator mediator,
        long requestNumber)
    {
        return await mediator.Send(new GetExpensesByRequestQuery { RequestNumber = requestNumber });
    }

    public async Task<ExpenseSummaryDto?> GetExpenseSummary(
        [Service] IMediator mediator,
        long requestNumber)
    {
        return await mediator.Send(new GetExpenseSummaryQuery { RequestNumber = requestNumber });
    }

    public async Task<DaSummaryDto?> GetDaSummary(
        [Service] IMediator mediator,
        long requestId)
    {
        return await mediator.Send(new GetDaSummaryQuery { RequestId = requestId });
    }

    public async Task<IReadOnlyList<ConveyanceDto>> GetConveyances(
        [Service] IMediator mediator,
        long requestNumber)
    {
        return await mediator.Send(new GetConveyancesByRequestQuery { RequestNumber = requestNumber });
    }

    public async Task<IReadOnlyList<SettlementDto>> GetSettlementReports(
        [Service] IMediator mediator,
        long requestNumber)
    {
        return await mediator.Send(new GetSettlementReportsQuery { RequestNumber = requestNumber });
    }
}
