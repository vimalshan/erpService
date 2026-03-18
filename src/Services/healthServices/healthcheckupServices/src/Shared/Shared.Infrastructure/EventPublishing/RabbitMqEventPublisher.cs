using MediatR;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Shared.Core.Domain;

namespace Shared.Infrastructure.EventPublishing;

/// <summary>
/// RabbitMQ implementation of event publisher for cross-service communication
/// </summary>
public class RabbitMqEventPublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private readonly IMediator _mediator;

    public RabbitMqEventPublisher(IConnection connection, ILogger<RabbitMqEventPublisher> logger, IMediator mediator)
    {
        _connection = connection;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Publish domain event to RabbitMQ and MediatR (for local handlers)
    /// </summary>
    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        try
        {
            // Publish to RabbitMQ for cross-service distribution
            var eventType = @event.GetType().Name;
            using var channel = _connection.CreateModel();

            // Declare exchange (topic type for flexible routing)
            channel.ExchangeDeclare(
                exchange: "health_services_events",
                type: ExchangeType.Topic,
                durable: true);

            // Serialize event
            var json = JsonSerializer.Serialize(@event);
            var body = System.Text.Encoding.UTF8.GetBytes(json);

            // Publish with routing key as event type
            var properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 2; // Persistent message
            properties.CorrelationId = @event.CorrelationId;
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            channel.BasicPublish(
                exchange: "health_services_events",
                routingKey: eventType,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published event {EventType} with ID {EventId} to RabbitMQ. CorrelationId: {CorrelationId}",
                eventType, @event.EventId, @event.CorrelationId);

            // Also publish locally via MediatR for local handlers
            await _mediator.Publish((INotification)@event, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event {EventType}", typeof(T).Name);
            throw;
        }
    }

    /// <summary>
    /// Publish multiple events in batch
    /// </summary>
    public async Task PublishBatchAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : DomainEvent
    {
        try
        {
            var eventList = events.ToList();
            foreach (var @event in eventList)
            {
                await PublishAsync(@event, cancellationToken);
            }

            _logger.LogInformation("Published batch of {Count} events", eventList.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing batch of events");
            throw;
        }
    }
}
