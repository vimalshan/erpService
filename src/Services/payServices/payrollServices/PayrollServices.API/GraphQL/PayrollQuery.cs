using PayrollServices.Application.DTOs;
using MediatR;
using PayrollServices.Application.Queries;

namespace PayrollServices.API.GraphQL;

/// <summary>
/// GraphQL Query type for payroll operations
/// </summary>
public class PayrollQuery
{
    public async Task<PayrollBatchDto?> GetBatch([Service] IMediator mediator, long batchId)
    {
        var query = new GetPayrollBatchByIdQuery { BatchId = batchId };
        return await mediator.Send(query);
    }

    public async Task<List<PayrollBatchDto>> GetAllBatches([Service] IMediator mediator)
    {
        var query = new GetAllPayrollBatchesQuery();
        var result = await mediator.Send(query);
        return result?.ToList() ?? new();
    }

    public async Task<List<PayrollTransactionDto>> GetBatchTransactions([Service] IMediator mediator, long batchId)
    {
        var query = new GetPayrollTransactionsByBatchQuery { BatchId = batchId };
        var result = await mediator.Send(query);
        return result?.ToList() ?? new();
    }
}
