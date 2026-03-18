using AlertsNotifications.Application.DTOs;
using AlertsNotifications.Application.Features.AlertGroups.Commands;
using AlertsNotifications.Application.Features.Alerts.Commands;
using AlertsNotifications.Application.Features.Circulars.Commands;
using AlertsNotifications.Application.Features.CircularTemplates.Commands;
using MediatR;

namespace AlertsNotifications.API.GraphQL;

public class AlertsMutation
{
    public async Task<AlertMasterDto> CreateAlert([Service] IMediator mediator, CreateAlertCommand input)
        => await mediator.Send(input);

    public async Task<bool> UpdateAlert([Service] IMediator mediator, UpdateAlertCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<bool> DeleteAlert([Service] IMediator mediator, decimal alertId)
    {
        await mediator.Send(new DeleteAlertCommand(alertId));
        return true;
    }

    public async Task<AlertGroupDto> CreateAlertGroup([Service] IMediator mediator, CreateAlertGroupCommand input)
        => await mediator.Send(input);

    public async Task<bool> UpdateAlertGroup([Service] IMediator mediator, UpdateAlertGroupCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<CircularDto> CreateCircular([Service] IMediator mediator, CreateCircularCommand input)
        => await mediator.Send(input);

    public async Task<bool> ApproveCircular([Service] IMediator mediator, ApproveCircularCommand input)
    {
        await mediator.Send(input);
        return true;
    }

    public async Task<CircularTemplateDto> CreateCircularTemplate([Service] IMediator mediator, CreateCircularTemplateCommand input)
        => await mediator.Send(input);
}
