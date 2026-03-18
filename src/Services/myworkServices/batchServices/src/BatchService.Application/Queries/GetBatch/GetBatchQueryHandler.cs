using AutoMapper;
using MediatR;
using BatchService.Application.DTOs;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Queries.GetBatch;

public sealed class GetBatchQueryHandler : IRequestHandler<GetBatchQuery, BatchDto?>
{
    private readonly IBatchRepository _repository;
    private readonly IMapper          _mapper;

    public GetBatchQueryHandler(IBatchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<BatchDto?> Handle(GetBatchQuery req, CancellationToken ct)
    {
        var batch = await _repository.GetByIdAsync(req.BatchId, ct);
        return batch is null ? null : _mapper.Map<BatchDto>(batch);
    }
}
