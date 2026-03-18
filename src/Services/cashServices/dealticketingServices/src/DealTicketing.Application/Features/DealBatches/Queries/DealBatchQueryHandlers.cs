using AutoMapper;
using DealTicketing.Application.DTOs;
using DealTicketing.Domain.Interfaces;
using MediatR;

namespace DealTicketing.Application.Features.DealBatches.Queries;

public class GetDealBatchByIdQueryHandler(IDealBatchRepository repository, IMapper mapper)
    : IRequestHandler<GetDealBatchByIdQuery, DealBatchDto?>
{
    public async Task<DealBatchDto?> Handle(GetDealBatchByIdQuery request, CancellationToken ct)
    {
        var batch = await repository.GetByIdAsync(request.DealBatchId, ct);
        return batch is null ? null : mapper.Map<DealBatchDto>(batch);
    }
}

public class GetDealBatchesByDateQueryHandler(IDealBatchRepository repository, IMapper mapper)
    : IRequestHandler<GetDealBatchesByDateQuery, IReadOnlyList<DealBatchDto>>
{
    public async Task<IReadOnlyList<DealBatchDto>> Handle(GetDealBatchesByDateQuery request, CancellationToken ct)
    {
        var batches = await repository.GetByDateAsync(request.Date, ct);
        return batches.Select(mapper.Map<DealBatchDto>).ToList();
    }
}
