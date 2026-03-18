namespace AccessService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using AccessService.Infrastructure.DomainEvents;

/// <summary>
/// Interceptor to handle domain events after saving changes to database
/// </summary>
public class DomainEventDispatcherInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly ILogger<DomainEventDispatcherInterceptor> _logger;

    public DomainEventDispatcherInterceptor(IDomainEventPublisher eventPublisher, ILogger<DomainEventDispatcherInterceptor> logger)
    {
        _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        // Get all domain events from aggregate roots
        var domainEvents = context.ChangeTracker
            .Entries()
            .Where(entry => entry.Entity is Domain.AggregateRoot)
            .SelectMany(entry => ((Domain.AggregateRoot)entry.Entity).DomainEvents)
            .ToList();

        if (domainEvents.Any())
        {
            _logger.LogInformation($"Publishing {domainEvents.Count} domain events");
            await _eventPublisher.PublishManyAsync(domainEvents, cancellationToken);

            // Clear domain events after publishing
            foreach (var entry in context.ChangeTracker
                .Entries()
                .Where(entry => entry.Entity is Domain.AggregateRoot))
            {
                ((Domain.AggregateRoot)entry.Entity).ClearDomainEvents();
            }
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
