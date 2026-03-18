using AutoMapper;
using MediatR;
using BatchService.Application.DTOs;
using BatchService.Domain.Interfaces;

namespace BatchService.Application.Queries.GetAllBatches;

public sealed class GetAllBatchesQueryHandler : IRequestHandler<GetAllBatchesQuery, IEnumerable<BatchDto>>
{
    private readonly IBatchRepository _repository;
    private readonly IMapper          _mapper;

    public GetAllBatchesQueryHandler(IBatchRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public async Task<IEnumerable<BatchDto>> Handle(GetAllBatchesQuery req, CancellationToken ct)
    {
        var batches = await _repository.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<BatchDto>>(batches);
    }
}
