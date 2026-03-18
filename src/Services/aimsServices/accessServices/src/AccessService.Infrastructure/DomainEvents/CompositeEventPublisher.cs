namespace AccessService.Infrastructure.DomainEvents;

using AccessService.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// Composite domain event publisher that publishes to both in-memory and RabbitMQ
/// In-memory handlers execute synchronously, RabbitMQ publishes asynchronously
/// </summary>
public class CompositeEventPublisher : IDomainEventPublisher
{
    private readonly InMemoryDomainEventPublisher _inMemoryPublisher;
    private readonly Messaging.IRabbitMQPublisher? _rabbitMQPublisher;
    private readonly ILogger<CompositeEventPublisher> _logger;

    public CompositeEventPublisher(
        InMemoryDomainEventPublisher inMemoryPublisher,
        Messaging.IRabbitMQPublisher? rabbitMQPublisher,
        ILogger<CompositeEventPublisher> logger)
    {
        _inMemoryPublisher = inMemoryPublisher ?? throw new ArgumentNullException(nameof(inMemoryPublisher));
        _rabbitMQPublisher = rabbitMQPublisher;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Publishes event to both in-memory handlers (synchronously) and RabbitMQ (asynchronously)
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent).Name;
        _logger.LogInformation($"Publishing event via composite publisher: {eventType}");

        // Publish to in-memory handlers (synchronous)
        await _inMemoryPublisher.PublishAsync(@event, cancellationToken);

        // Publish to RabbitMQ if configured (asynchronous, fire-and-forget)
        if (_rabbitMQPublisher != null)
        {
            try
            {
                #pragma warning disable CS4014
                _rabbitMQPublisher.PublishEventAsync(@event);
                #pragma warning restore CS4014
                _logger.LogInformation($"Event published to RabbitMQ: {eventType}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Failed to publish event to RabbitMQ: {ex.Message}");
                // Don't throw - continue execution even if RabbitMQ fails
            }
        }
    }

    /// <summary>
    /// Publishes multiple events to both in-memory handlers and RabbitMQ
    /// </summary>
    public async Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var publishMethod = typeof(CompositeEventPublisher)
                .GetMethod("PublishAsync")!
                .MakeGenericMethod(@event.GetType());

            await (Task)publishMethod.Invoke(this, new object[] { @event, cancellationToken })!;
        }
    }
}
