using MediatR;
using BatchAndEnvelopeService.Application.DTOs;
using BatchAndEnvelopeService.Domain.Aggregates;
using BatchAndEnvelopeService.Domain.Interfaces;

namespace BatchAndEnvelopeService.Application.Queries.Envelope;

public class GetEnvelopeByIdQueryHandler : IRequestHandler<GetEnvelopeByIdQuery, EnvelopeDto?>
{
    private readonly IEnvelopeRepository _repository;
    public GetEnvelopeByIdQueryHandler(IEnvelopeRepository repository) => _repository = repository;

    public async Task<EnvelopeDto?> Handle(GetEnvelopeByIdQuery request, CancellationToken cancellationToken)
    {
        var envelope = await _repository.GetByIdAsync(request.EnvelopeId, cancellationToken);
        return envelope is null ? null : MapToDto(envelope);
    }

    private static EnvelopeDto MapToDto(EnvelopeAggregate e) => new(
        e.Id, e.EnvelopeType, e.CreatedBy, e.CreatedOn, e.ReceivedBy, e.ReceivedOn,
        e.SummaryFlag, e.CancelledBy, e.CancelledOn, e.ConfirmedBy, e.ConfirmedOn,
        e.ScanLotNo, e.LocationId,
        e.Details.Select(d => new EnvelopeDetailDto(d.Id, d.EnvelopeId, d.EnvelopeType, d.DocumentId, d.CreatedBy, d.CreatedOn, d.ReceiveFlag))
    );
}

public class GetAllEnvelopesQueryHandler : IRequestHandler<GetAllEnvelopesQuery, IEnumerable<EnvelopeDto>>
{
    private readonly IEnvelopeRepository _repository;
    public GetAllEnvelopesQueryHandler(IEnvelopeRepository repository) => _repository = repository;

    public async Task<IEnumerable<EnvelopeDto>> Handle(GetAllEnvelopesQuery request, CancellationToken cancellationToken)
    {
        var envelopes = await _repository.GetAllAsync(cancellationToken);
        return envelopes
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EnvelopeDto(e.Id, e.EnvelopeType, e.CreatedBy, e.CreatedOn, e.ReceivedBy, e.ReceivedOn,
                e.SummaryFlag, e.CancelledBy, e.CancelledOn, e.ConfirmedBy, e.ConfirmedOn, e.ScanLotNo, e.LocationId, []));
    }
}

public class GetEnvelopesByTypeQueryHandler : IRequestHandler<GetEnvelopesByTypeQuery, IEnumerable<EnvelopeDto>>
{
    private readonly IEnvelopeRepository _repository;
    public GetEnvelopesByTypeQueryHandler(IEnvelopeRepository repository) => _repository = repository;

    public async Task<IEnumerable<EnvelopeDto>> Handle(GetEnvelopesByTypeQuery request, CancellationToken cancellationToken)
    {
        var envelopes = await _repository.GetByTypeAsync(request.EnvelopeType, cancellationToken);
        return envelopes.Select(e => new EnvelopeDto(e.Id, e.EnvelopeType, e.CreatedBy, e.CreatedOn, e.ReceivedBy, e.ReceivedOn,
            e.SummaryFlag, e.CancelledBy, e.CancelledOn, e.ConfirmedBy, e.ConfirmedOn, e.ScanLotNo, e.LocationId, []));
    }
}
