using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.Alerts.Queries;

public record GetAllAlertsQuery : IRequest<IEnumerable<AlertMasterDto>>;

public record GetAlertByIdQuery(decimal AlertId) : IRequest<AlertMasterDto?>;

public record GetAlertsByAppQuery(string AlertApps) : IRequest<IEnumerable<AlertMasterDto>>;
