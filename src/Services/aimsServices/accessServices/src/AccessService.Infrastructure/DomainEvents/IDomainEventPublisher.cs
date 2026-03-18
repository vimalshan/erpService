namespace AccessService.Infrastructure.DomainEvents;

using Microsoft.Extensions.Logging;
using AccessService.Domain;

/// <summary>
/// Domain Event Publisher abstraction
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
    Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}

/// <summary>
/// Domain Event Handler abstraction
/// </summary>
public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

/// <summary>
/// In-memory implementation of Domain Event Publisher
/// Can be replaced with RabbitMQ or other message bus implementations
/// </summary>
public class InMemoryDomainEventPublisher : IDomainEventPublisher
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    private readonly ILogger<InMemoryDomainEventPublisher> _logger;

    public InMemoryDomainEventPublisher(ILogger<InMemoryDomainEventPublisher> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Subscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);
        if (!_handlers.ContainsKey(eventType))
        {
            _handlers[eventType] = new List<Delegate>();
        }

        _handlers[eventType].Add(handler.HandleAsync);
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
    {
        var eventType = typeof(TEvent);
        _logger.LogInformation($"Publishing domain event: {eventType.Name}");

        if (_handlers.TryGetValue(eventType, out var handlerList))
        {
            var tasks = handlerList
                .Cast<Func<TEvent, CancellationToken, Task>>()
                .Select(handler => handler(@event, cancellationToken))
                .ToList();

            await Task.WhenAll(tasks);
            _logger.LogInformation($"Domain event published: {eventType.Name}, Handlers: {handlerList.Count}");
        }
        else
        {
            _logger.LogWarning($"No handlers found for event type: {eventType.Name}");
        }
    }

    public async Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            var publishMethod = typeof(InMemoryDomainEventPublisher)
                .GetMethod("PublishAsync")!
                .MakeGenericMethod(@event.GetType());

            await (Task)publishMethod.Invoke(this, new object[] { @event, cancellationToken })!;
        }
    }
}
