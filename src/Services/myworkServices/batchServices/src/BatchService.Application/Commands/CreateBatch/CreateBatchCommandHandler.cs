using AutoMapper;
using MediatR;
using BatchService.Application.DTOs;
using BatchService.Domain.Entities;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Commands.CreateBatch;

public sealed class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, BatchDto>
{
    private readonly IBatchRepository  _repository;
    private readonly IMessagePublisher _publisher;
    private readonly IMapper           _mapper;

    public CreateBatchCommandHandler(IBatchRepository repository, IMessagePublisher publisher, IMapper mapper)
    {
        _repository = repository;
        _publisher  = publisher;
        _mapper     = mapper;
    }

    public async Task<BatchDto> Handle(CreateBatchCommand cmd, CancellationToken ct)
    {
        if (await _repository.ExistsAsync(cmd.BatchId, ct))
            throw new InvalidOperationException($"Batch {cmd.BatchId} already exists.");

        var batch = BatchMaster.Create(cmd.BatchId, cmd.MonthNo, cmd.ModifiedBy);
        await _repository.AddAsync(batch, ct);

        // Dispatch domain events (fire-and-forget to message bus)
        foreach (var evt in batch.DomainEvents)
            await _publisher.PublishAsync(evt, evt.EventType, ct);

        batch.ClearDomainEvents();
        return _mapper.Map<BatchDto>(batch);
    }
}
