using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ReferenceService.Domain;
using ReferenceService.Domain.Interfaces;

namespace ReferenceService.Infrastructure.DomainEventPublisher;

/// <summary>
/// RabbitMQ implementation of IDomainEventPublisher.
/// </summary>
public class RabbitMQDomainEventPublisher : IDomainEventPublisher
{
    private readonly IConnection _connection;
    private readonly dynamic _channel;
    
    public RabbitMQDomainEventPublisher(IConnection connection)
    {
        _connection = connection;
        _channel = connection.CreateModel();
        
        // Declare exchange for domain events
        _channel.ExchangeDeclare(exchange: "domain-events", type: ExchangeType.Topic, durable: true);
    }
    
    public async Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(domainEvent);
        var body = Encoding.UTF8.GetBytes(json);
        
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        
        var eventTypeName = domainEvent.GetType().Name;
        var routingKey = $"domain-event.{eventTypeName}";
        
        _channel.BasicPublish(
            exchange: "domain-events",
            routingKey: routingKey,
            basicProperties: properties,
            body: body
        );
        
        await Task.CompletedTask;
    }
    
    public async Task PublishMultipleAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await PublishAsync(domainEvent, cancellationToken);
        }
    }
}
