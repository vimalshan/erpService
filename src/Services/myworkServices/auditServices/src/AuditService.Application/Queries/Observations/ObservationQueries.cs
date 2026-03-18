using AuditService.Application.DTOs;
using MediatR;

namespace AuditService.Application.Queries.Observations;

public record GetObservationByIdQuery(long ObvId) : IRequest<ObservationDto?>;

public record GetObservationsByAuditQuery(long AuditId) : IRequest<IEnumerable<ObservationDto>>;

public record GetPendingObservationsQuery : IRequest<IEnumerable<ObservationDto>>;
