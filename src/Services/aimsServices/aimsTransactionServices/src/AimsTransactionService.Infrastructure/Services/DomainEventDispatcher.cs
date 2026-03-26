using MediatR;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Domain.Common;
using AimsTransactionService.Infrastructure.Data;

namespace AimsTransactionService.Infrastructure.Services;

public class DomainEventDispatcher(
    IPublisher publisher,
    AimsTransactionDbContext dbContext,
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
