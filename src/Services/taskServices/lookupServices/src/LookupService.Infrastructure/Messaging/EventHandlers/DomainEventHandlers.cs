using LookupService.Application.Interfaces;
using LookupService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LookupService.Infrastructure.Messaging.EventHandlers;

public class LovCreatedEventHandler(IMessagePublisher publisher, ILogger<LovCreatedEventHandler> logger)
    : INotificationHandler<LovCreatedEvent>
{
    public async Task Handle(LovCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: LOV Created - ID {LovId}, Type {LovType}", notification.LovId, notification.LovType);
        await publisher.PublishAsync("lookup.exchange", "lov.created", notification, ct);
    }
}

public class LovUpdatedEventHandler(IMessagePublisher publisher, ILogger<LovUpdatedEventHandler> logger)
    : INotificationHandler<LovUpdatedEvent>
{
    public async Task Handle(LovUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: LOV Updated - ID {LovId}", notification.LovId);
        await publisher.PublishAsync("lookup.exchange", "lov.updated", notification, ct);
    }
}

public class ProcessCreatedEventHandler(IMessagePublisher publisher, ILogger<ProcessCreatedEventHandler> logger)
    : INotificationHandler<ProcessCreatedEvent>
{
    public async Task Handle(ProcessCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Process Created - ID {ProcessId}", notification.ProcessId);
        await publisher.PublishAsync("lookup.exchange", "process.created", notification, ct);
    }
}

public class ProcessUpdatedEventHandler(IMessagePublisher publisher, ILogger<ProcessUpdatedEventHandler> logger)
    : INotificationHandler<ProcessUpdatedEvent>
{
    public async Task Handle(ProcessUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Process Updated - ID {ProcessId}", notification.ProcessId);
        await publisher.PublishAsync("lookup.exchange", "process.updated", notification, ct);
    }
}

public class AccessMasterCreatedEventHandler(IMessagePublisher publisher, ILogger<AccessMasterCreatedEventHandler> logger)
    : INotificationHandler<AccessMasterCreatedEvent>
{
    public async Task Handle(AccessMasterCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Access Master Created - ID {AccessMastId}", notification.AccessMastId);
        await publisher.PublishAsync("lookup.exchange", "accessmaster.created", notification, ct);
    }
}
