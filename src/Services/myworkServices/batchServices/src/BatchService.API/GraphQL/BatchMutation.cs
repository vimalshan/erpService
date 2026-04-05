using MediatR;
using BatchService.Application.Commands.CloseBatch;
using BatchService.Application.Commands.CreateBatch;
using BatchService.Application.Commands.DeleteBatch;
using BatchService.Application.Commands.UpdateBatch;
using BatchService.Application.DTOs;

namespace BatchService.API.GraphQL;

public sealed class BatchMutation
{
    public async Task<BatchDto> CreateBatch(
        long batchId, int monthNo, long modifiedBy,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new CreateBatchCommand(batchId, monthNo, modifiedBy), ct);

    public async Task<BatchDto> UpdateBatch(
        long batchId, int monthNo, long modifiedBy,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new UpdateBatchCommand(batchId, monthNo, modifiedBy), ct);

    public async Task<bool> CloseBatch(
        long batchId, long modifiedBy,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new CloseBatchCommand(batchId, modifiedBy), ct);
        return true;
    }

    public async Task<bool> DeleteBatch(
        long batchId,
        [Service] IMediator mediator,
        CancellationToken ct)
    {
        await mediator.Send(new DeleteBatchCommand(batchId), ct);
        return true;
    }
}
