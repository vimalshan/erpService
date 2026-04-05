using AuditService.Application.DTOs;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using MediatR;

namespace AuditService.Application.Queries.Observations;

public sealed class GetObservationByIdQueryHandler : IRequestHandler<GetObservationByIdQuery, ObservationDto?>
{
    private readonly IObservationRepository _repository;

    public GetObservationByIdQueryHandler(IObservationRepository repository) => _repository = repository;

    public async Task<ObservationDto?> Handle(GetObservationByIdQuery request, CancellationToken cancellationToken)
    {
        var obs = await _repository.GetByIdAsync(request.ObvId, cancellationToken);
        return obs is null ? null : ToDto(obs);
    }

    private static ObservationDto ToDto(AuditObservation o) => new(
        o.ObvId, o.ObvAuditId, o.ObvTitle, o.ObvDescription, o.ObvRisk.ToString(),
        o.ObvAuditee, o.ObvEsc1, o.ObvEsc2, o.ObvManComments, o.ObvImplication,
        o.ObvStatus.ToString(), o.ObvOrgDueDate, o.ObvOrgRev1Date, o.ObvOrgRev2Date,
        o.ObvCreatedBy, o.ObvCreatedOn, o.ObvLocation, o.ObvAuditorName,
        o.ObvRemarks, o.ObvAppStatus?.ToString());
}

public sealed class GetObservationsByAuditQueryHandler : IRequestHandler<GetObservationsByAuditQuery, IEnumerable<ObservationDto>>
{
    private readonly IObservationRepository _repository;

    public GetObservationsByAuditQueryHandler(IObservationRepository repository) => _repository = repository;

    public async Task<IEnumerable<ObservationDto>> Handle(GetObservationsByAuditQuery request, CancellationToken cancellationToken)
    {
        var observations = await _repository.GetByAuditIdAsync(request.AuditId, cancellationToken);
        return observations.Select(o => new ObservationDto(
            o.ObvId, o.ObvAuditId, o.ObvTitle, o.ObvDescription, o.ObvRisk.ToString(),
            o.ObvAuditee, o.ObvEsc1, o.ObvEsc2, o.ObvManComments, o.ObvImplication,
            o.ObvStatus.ToString(), o.ObvOrgDueDate, o.ObvOrgRev1Date, o.ObvOrgRev2Date,
            o.ObvCreatedBy, o.ObvCreatedOn, o.ObvLocation, o.ObvAuditorName,
            o.ObvRemarks, o.ObvAppStatus?.ToString()));
    }
}

public sealed class GetPendingObservationsQueryHandler : IRequestHandler<GetPendingObservationsQuery, IEnumerable<ObservationDto>>
{
    private readonly IObservationRepository _repository;

    public GetPendingObservationsQueryHandler(IObservationRepository repository) => _repository = repository;

    public async Task<IEnumerable<ObservationDto>> Handle(GetPendingObservationsQuery request, CancellationToken cancellationToken)
    {
        var observations = await _repository.GetPendingObservationsAsync(cancellationToken);
        return observations.Select(o => new ObservationDto(
            o.ObvId, o.ObvAuditId, o.ObvTitle, o.ObvDescription, o.ObvRisk.ToString(),
            o.ObvAuditee, o.ObvEsc1, o.ObvEsc2, o.ObvManComments, o.ObvImplication,
            o.ObvStatus.ToString(), o.ObvOrgDueDate, o.ObvOrgRev1Date, o.ObvOrgRev2Date,
            o.ObvCreatedBy, o.ObvCreatedOn, o.ObvLocation, o.ObvAuditorName,
            o.ObvRemarks, o.ObvAppStatus?.ToString()));
    }
}
