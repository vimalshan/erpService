using Recruitment.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Recruitment.Infrastructure.EventPublishing;

/// <summary>
/// Interface for publishing domain events
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<T>(T @event) where T : DomainEvent;
    Task PublishBatchAsync<T>(IEnumerable<T> events) where T : DomainEvent;
}

/// <summary>
/// Simple in-memory event publisher implementation
/// Can be enhanced with RabbitMQ, Azure Service Bus, or other pub/sub systems later
/// </summary>
public class InMemoryEventPublisher : IEventPublisher
{
    private readonly ILogger<InMemoryEventPublisher> _logger;
    private static List<DomainEvent> _publishedEvents = new();

    public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event) where T : DomainEvent
    {
        try
        {
            _publishedEvents.Add(@event);
            _logger.LogInformation($"Published event: {typeof(T).Name}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error publishing event: {typeof(T).Name}");
            throw;
        }

        await Task.CompletedTask;
    }

    public async Task PublishBatchAsync<T>(IEnumerable<T> events) where T : DomainEvent
    {
        foreach (var @event in events)
        {
            await PublishAsync(@event);
        }
    }

    public static IReadOnlyList<DomainEvent> GetPublishedEvents() => _publishedEvents.AsReadOnly();

    public static void ClearPublishedEvents() => _publishedEvents.Clear();
}
