using MediatR;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Interfaces;

namespace BatchAndEnvelopeService.Application.Queries.Batch;

public class GetBatchByIdQueryHandler : IRequestHandler<GetBatchByIdQuery, BatchDto?>
{
    private readonly IBatchRepository _repository;
    public GetBatchByIdQueryHandler(IBatchRepository repository) => _repository = repository;

    public async Task<BatchDto?> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await _repository.GetByIdAsync(request.BatchId, cancellationToken);
        return batch is null ? null : MapToDto(batch);
    }

    private static BatchDto MapToDto(BatchAggregate b) => new(
        b.Id, b.CreatedBy, b.CreatedOn, b.LocationId, b.ReceivedBy, b.ReceivedOn,
        b.PodNo, b.SummaryFlag, b.CancelBy, b.CancelDate, b.ConfirmedBy, b.ConfirmedOn,
        b.CourierName, b.ScanFlag,
        b.Details.Select(d => new BatchDetailDto(d.Id, d.BatchId, d.EnvelopeId, d.CreatedBy, d.CreatedOn, d.ReceiveFlag, d.ReceivedBy, d.ReceivedOn))
    );
}

public class GetAllBatchesQueryHandler : IRequestHandler<GetAllBatchesQuery, IEnumerable<BatchDto>>
{
    private readonly IBatchRepository _repository;
    public GetAllBatchesQueryHandler(IBatchRepository repository) => _repository = repository;

    public async Task<IEnumerable<BatchDto>> Handle(GetAllBatchesQuery request, CancellationToken cancellationToken)
    {
        var batches = await _repository.GetAllAsync(cancellationToken);
        return batches
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(b => new BatchDto(b.Id, b.CreatedBy, b.CreatedOn, b.LocationId, b.ReceivedBy, b.ReceivedOn,
                b.PodNo, b.SummaryFlag, b.CancelBy, b.CancelDate, b.ConfirmedBy, b.ConfirmedOn, b.CourierName, b.ScanFlag, []));
    }
}

public class GetBatchesByLocationQueryHandler : IRequestHandler<GetBatchesByLocationQuery, IEnumerable<BatchDto>>
{
    private readonly IBatchRepository _repository;
    public GetBatchesByLocationQueryHandler(IBatchRepository repository) => _repository = repository;

    public async Task<IEnumerable<BatchDto>> Handle(GetBatchesByLocationQuery request, CancellationToken cancellationToken)
    {
        var batches = await _repository.GetByLocationAsync(request.LocationId, cancellationToken);
        return batches.Select(b => new BatchDto(b.Id, b.CreatedBy, b.CreatedOn, b.LocationId, b.ReceivedBy, b.ReceivedOn,
            b.PodNo, b.SummaryFlag, b.CancelBy, b.CancelDate, b.ConfirmedBy, b.ConfirmedOn, b.CourierName, b.ScanFlag, []));
    }
}
