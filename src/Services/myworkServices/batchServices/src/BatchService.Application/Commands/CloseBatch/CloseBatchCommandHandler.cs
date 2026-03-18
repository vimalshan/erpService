using MediatR;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Commands.CloseBatch;

public sealed class CloseBatchCommandHandler : IRequestHandler<CloseBatchCommand>
{
    private readonly IBatchRepository  _repository;
    private readonly IMessagePublisher _publisher;

    public CloseBatchCommandHandler(IBatchRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task Handle(CloseBatchCommand cmd, CancellationToken ct)
    {
        var batch = await _repository.GetByIdAsync(cmd.BatchId, ct)
                    ?? throw new KeyNotFoundException($"Batch {cmd.BatchId} not found.");

        batch.Close(cmd.ModifiedBy);
        await _repository.UpdateAsync(batch, ct);

        foreach (var evt in batch.DomainEvents)
            await _publisher.PublishAsync(evt, evt.EventType, ct);

        batch.ClearDomainEvents();
    }
}
