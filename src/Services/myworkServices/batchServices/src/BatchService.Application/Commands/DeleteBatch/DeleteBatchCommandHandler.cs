using MediatR;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Commands.DeleteBatch;

public sealed class DeleteBatchCommandHandler : IRequestHandler<DeleteBatchCommand>
{
    private readonly IBatchRepository _repository;

    public DeleteBatchCommandHandler(IBatchRepository repository) => _repository = repository;

    public async Task Handle(DeleteBatchCommand cmd, CancellationToken ct)
    {
        if (!await _repository.ExistsAsync(cmd.BatchId, ct))
            throw new KeyNotFoundException($"Batch {cmd.BatchId} not found.");

        await _repository.DeleteAsync(cmd.BatchId, ct);
    }
}
