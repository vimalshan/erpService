namespace CheckupManagementService.Infrastructure.EventPublishing;

using Microsoft.Extensions.Logging;
using Shared.Events;

/// <summary>
/// Implementation of domain event publisher using in-memory handling
/// </summary>
public class DomainEventPublisher : IEventPublisher
{
    private readonly ILogger<DomainEventPublisher> _logger;

    public DomainEventPublisher(ILogger<DomainEventPublisher> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Publish a domain event
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : DomainEvent
    {
        try
        {
            _logger.LogInformation(
                "Publishing domain event: {EventName} at {Timestamp}",
                @event.GetType().Name,
                DateTime.UtcNow);

            // TODO: Implement actual event publishing to RabbitMQ or message queue
            // For now, just log the event has been processed
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain event: {EventName}", @event.GetType().Name);
            throw;
        }
    }

    /// <summary>
    /// Publish batch of domain events
    /// </summary>
    public async Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken cancellationToken = default) 
        where TEvent : DomainEvent
    {
        try
        {
            var eventList = events.ToList();
            _logger.LogInformation(
                "Publishing batch of {Count} domain events at {Timestamp}",
                eventList.Count,
                DateTime.UtcNow);

            foreach (var @event in eventList)
            {
                await PublishAsync(@event, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing batch of domain events");
            throw;
        }
    }
}
