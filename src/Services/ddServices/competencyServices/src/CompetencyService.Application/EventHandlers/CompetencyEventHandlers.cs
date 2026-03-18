using MediatR;
using Microsoft.Extensions.Logging;
using CompetencyService.Domain.Events;

namespace CompetencyService.Application.EventHandlers;

public class CompetencyCreatedEventHandler(ILogger<CompetencyCreatedEventHandler> logger)
    : INotificationHandler<CompetencyCreatedDomainEventNotification>
{
    public Task Handle(CompetencyCreatedDomainEventNotification notification, CancellationToken ct)
    {
        logger.LogInformation("Competency created: Id={Id} Name={Name}",
            notification.Event.CompetencyId, notification.Event.Name);
        return Task.CompletedTask;
    }
}

public class CompetencyUpdatedEventHandler(ILogger<CompetencyUpdatedEventHandler> logger)
    : INotificationHandler<CompetencyUpdatedDomainEventNotification>
{
    public Task Handle(CompetencyUpdatedDomainEventNotification notification, CancellationToken ct)
    {
        logger.LogInformation("Competency updated: Id={Id} Name={Name}",
            notification.Event.CompetencyId, notification.Event.Name);
        return Task.CompletedTask;
    }
}
