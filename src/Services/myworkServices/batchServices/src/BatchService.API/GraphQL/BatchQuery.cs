using MediatR;
using BatchService.Application.DTOs;
using BatchService.Application.Queries.GetAllBatches;
using BatchService.Application.Queries.GetBatch;
using BatchService.Application.Queries.GetBatchesByMonth;

namespace BatchService.API.GraphQL;

[QueryType]
public sealed class BatchQuery
{
    public async Task<IEnumerable<BatchDto>> GetBatches(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetAllBatchesQuery(), ct);

    public async Task<BatchDto?> GetBatchById(
        long id,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetBatchQuery(id), ct);

    public async Task<IEnumerable<BatchDto>> GetBatchesByMonth(
        int monthNo,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetBatchesByMonthQuery(monthNo), ct);
}
