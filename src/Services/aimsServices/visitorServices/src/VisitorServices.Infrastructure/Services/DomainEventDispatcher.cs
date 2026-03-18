using MediatR;
using Microsoft.Extensions.Logging;
using VisitorServices.Domain.Common;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Services;

/// <summary>
/// Dispatches collected domain events via MediatR after EF persistence.
/// </summary>
public class DomainEventDispatcher(
    IPublisher publisher,
    VisitorDbContext dbContext,
    ILogger<DomainEventDispatcher> logger)
{
    public async Task DispatchAsync(CancellationToken cancellationToken = default)
    {
        var events = dbContext.DomainEventsBeforeSave;
        if (events.Count == 0) return;

        foreach (var domainEvent in events)
        {
            logger.LogDebug("Dispatching domain event {EventType}", domainEvent.GetType().Name);
            await publisher.Publish((INotification)domainEvent, cancellationToken);
        }

        dbContext.DomainEventsBeforeSave.Clear();
    }
}
