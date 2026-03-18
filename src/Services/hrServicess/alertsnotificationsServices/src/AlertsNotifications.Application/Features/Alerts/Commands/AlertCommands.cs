using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.Alerts.Commands;

public record CreateAlertCommand(
    decimal AlertId,
    string AlertApps,
    string AlertName,
    string AlertType,
    string AlertDesc,
    string? AlertToDesc,
    string? AlertCcDesc,
    string? AlertGradeCat,
    char? AlertUnitSpecific
) : IRequest<AlertMasterDto>;

public record UpdateAlertCommand(
    decimal AlertId,
    string AlertApps,
    string AlertName,
    string AlertType,
    string AlertDesc,
    string? AlertToDesc,
    string? AlertCcDesc,
    string? AlertGradeCat,
    char? AlertUnitSpecific
) : IRequest<Unit>;

public record DeleteAlertCommand(decimal AlertId) : IRequest<Unit>;
