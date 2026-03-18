using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.AlertGroups.Queries;
using AlertsNotifications.Application.Features.Alerts.Queries;
using AlertsNotifications.Application.Features.Circulars.Queries;
using AlertsNotifications.Application.Features.CircularTemplates.Queries;
using MediatR;

namespace AlertsNotifications.API.GraphQL;

public class AlertsQuery
{
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<AlertMasterDto>> GetAlerts([Service] IMediator mediator)
        => await mediator.Send(new GetAllAlertsQuery());

    public async Task<AlertMasterDto?> GetAlertById([Service] IMediator mediator, decimal alertId)
        => await mediator.Send(new GetAlertByIdQuery(alertId));

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<AlertGroupDto>> GetAlertGroups([Service] IMediator mediator)
        => await mediator.Send(new GetAllAlertGroupsQuery());

    public async Task<AlertGroupDto?> GetAlertGroupById([Service] IMediator mediator, decimal alertGroupId)
        => await mediator.Send(new GetAlertGroupByIdQuery(alertGroupId));

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<CircularDto>> GetCirculars([Service] IMediator mediator)
        => await mediator.Send(new GetAllCircularsQuery());

    public async Task<CircularDto?> GetCircularById([Service] IMediator mediator, long circularId)
        => await mediator.Send(new GetCircularByIdQuery(circularId));

    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<CircularTemplateDto>> GetCircularTemplates([Service] IMediator mediator)
        => await mediator.Send(new GetAllCircularTemplatesQuery());

    public async Task<CircularTemplateDto?> GetCircularTemplateById([Service] IMediator mediator, long templateId)
        => await mediator.Send(new GetCircularTemplateByIdQuery(templateId));
}
