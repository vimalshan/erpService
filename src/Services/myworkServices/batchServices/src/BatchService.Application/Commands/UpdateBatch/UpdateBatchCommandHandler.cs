using AutoMapper;
using MediatR;
using BatchService.Application.DTOs;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Commands.UpdateBatch;

public sealed class UpdateBatchCommandHandler : IRequestHandler<UpdateBatchCommand, BatchDto>
{
    private readonly IBatchRepository _repository;
    private readonly IMapper          _mapper;

    public UpdateBatchCommandHandler(IBatchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<BatchDto> Handle(UpdateBatchCommand cmd, CancellationToken ct)
    {
        var batch = await _repository.GetByIdAsync(cmd.BatchId, ct)
                    ?? throw new KeyNotFoundException($"Batch {cmd.BatchId} not found.");

        batch.UpdateMonth(cmd.MonthNo, cmd.ModifiedBy);
        await _repository.UpdateAsync(batch, ct);
        return _mapper.Map<BatchDto>(batch);
    }
}
