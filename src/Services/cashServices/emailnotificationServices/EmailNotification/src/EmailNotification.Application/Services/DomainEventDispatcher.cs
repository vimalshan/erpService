using MediatR;
using Microsoft.Extensions.Logging;
using EmailNotification.Domain.Common;

namespace EmailNotification.Application.Services;

/// <summary>
/// Service for dispatching domain events from aggregates
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches all domain events from an aggregate
    /// </summary>
    /// <param name="aggregate">The aggregate containing domain events</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DispatchEventsAsync(Entity aggregate, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of domain event dispatcher
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Dispatches all domain events from an aggregate to registered handlers
    /// </summary>
    public async Task DispatchEventsAsync(Entity aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate?.DomainEvents == null || !aggregate.DomainEvents.Any())
        {
            return;
        }

        var events = aggregate.DomainEvents.ToList();
        var aggregateId = aggregate.Id;

        _logger.LogInformation(
            "Dispatching {EventCount} domain event(s) for aggregate {AggregateType} (ID: {AggregateId})",
            events.Count,
            aggregate.GetType().Name,
            aggregateId);

        foreach (var domainEvent in events)
        {
            try
            {
                _logger.LogDebug(
                    "Publishing domain event {EventType}: {EventDetails}",
                    domainEvent.GetType().Name,
                    domainEvent);

                // Publish the domain event to registered handlers (MediatR INotificationHandler)
                await _mediator.Publish((INotification)domainEvent, cancellationToken);

                _logger.LogInformation(
                    "Successfully published domain event {EventType} for aggregate {AggregateType} (ID: {AggregateId})",
                    domainEvent.GetType().Name,
                    aggregate.GetType().Name,
                    aggregateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error publishing domain event {EventType} for aggregate {AggregateType} (ID: {AggregateId})",
                    domainEvent.GetType().Name,
                    aggregate.GetType().Name,
                    aggregateId);

                // Re-throw to maintain consistency
                throw;
            }
        }

        // Clear dispatched events
        aggregate.ClearDomainEvents();
        _logger.LogDebug("Cleared {EventCount} domain event(s) from aggregate", events.Count);
    }
}
