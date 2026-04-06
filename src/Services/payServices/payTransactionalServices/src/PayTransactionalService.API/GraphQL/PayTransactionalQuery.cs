using MediatR;
using PayTransactionalService.Application.DTOs;
using PayTransactionalService.Application.Queries;

namespace PayTransactionalService.API.GraphQL;

public class PayTransactionalQuery
{
    public async Task<IEnumerable<PayTransactionDto>> GetPayTransactionsByEmployee(
        [Service] IMediator mediator, long employeeSystemId)
    {
        var result = await mediator.Send(new GetPayTransactionsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayTransactionDto>();
    }

    public async Task<PayTransactionDto?> GetPayTransactionById(
        [Service] IMediator mediator, long id)
    {
        var result = await mediator.Send(new GetPayTransactionByIdQuery(id));
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IEnumerable<PayTransactionDto>> GetPayTransactionsByMonth(
        [Service] IMediator mediator, string monthYear)
    {
        var result = await mediator.Send(new GetPayTransactionsByMonthQuery(monthYear));
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayTransactionDto>();
    }

    public async Task<IEnumerable<PayArrearDto>> GetPayArrearsByEmployee(
        [Service] IMediator mediator, long employeeSystemId)
    {
        var result = await mediator.Send(new GetPayArrearsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayArrearDto>();
    }

    public async Task<PayArrearDto?> GetPayArrearById(
        [Service] IMediator mediator, long id)
    {
        var result = await mediator.Send(new GetPayArrearByIdQuery(id));
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<IEnumerable<PayArrearDto>> GetUnprocessedArrears(
        [Service] IMediator mediator, long employeeSystemId)
    {
        var result = await mediator.Send(new GetUnprocessedArrearsQuery(employeeSystemId));
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayArrearDto>();
    }

    public async Task<IEnumerable<PayAdjustmentDto>> GetPayAdjustmentsByEmployee(
        [Service] IMediator mediator, long employeeSystemId)
    {
        var result = await mediator.Send(new GetPayAdjustmentsByEmployeeQuery(employeeSystemId));
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayAdjustmentDto>();
    }

    public async Task<IEnumerable<PayAdjustmentDto>> GetPendingAdjustments(
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new GetPendingAdjustmentsQuery());
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayAdjustmentDto>();
    }

    public async Task<IEnumerable<PayrollBatchDto>> GetAllPayrollBatches(
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new GetAllPayrollBatchesQuery());
        return result.IsSuccess ? result.Data! : Enumerable.Empty<PayrollBatchDto>();
    }

    public async Task<PayrollBatchDto?> GetPayrollBatchById(
        [Service] IMediator mediator, long id)
    {
        var result = await mediator.Send(new GetPayrollBatchByIdQuery(id));
        return result.IsSuccess ? result.Data : null;
    }
}
