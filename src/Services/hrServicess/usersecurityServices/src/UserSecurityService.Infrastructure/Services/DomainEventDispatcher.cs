using MediatR;
using Microsoft.Extensions.Logging;
using UserSecurityService.Application.Common;
using UserSecurityService.Domain.Common;

namespace UserSecurityService.Infrastructure.Services;

public sealed class DomainEventDispatcher(IPublisher publisher, ILogger<DomainEventDispatcher> logger)
    : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var domainEvent in events)
        {
            logger.LogDebug("Dispatching domain event {EventType}: {EventId}", domainEvent.GetType().Name, domainEvent.EventId);
            // Wrap domain event as MediatR notification
            var notification = new DomainEventNotification(domainEvent);
            await publisher.Publish(notification, ct);
        }
    }
}

public sealed record DomainEventNotification(IDomainEvent DomainEvent) : INotification;
