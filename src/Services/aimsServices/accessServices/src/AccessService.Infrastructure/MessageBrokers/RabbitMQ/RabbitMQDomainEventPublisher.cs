using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessService.Infrastructure.DomainEvents;
using AccessService.Domain;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// Domain event publisher that sends events through RabbitMQ
    /// Implements IDomainEventPublisher for integration with domain models
    /// </summary>
    public class RabbitMQDomainEventPublisher : IDomainEventPublisher
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly ILogger<RabbitMQDomainEventPublisher> _logger;
        private const string ExchangeName = "access-service-exchange";

        public RabbitMQDomainEventPublisher(IRabbitMQPublisher publisher, ILogger<RabbitMQDomainEventPublisher> logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent
        {
            try
            {
                var eventType = typeof(TEvent).Name;
                var routingKey = GetRoutingKey(eventType);
                var message = JsonSerializer.Serialize(@event);
                var messageId = Guid.NewGuid().ToString();

                var headers = new Dictionary<string, object>
                {
                    { "EventType", eventType },
                    { "MessageId", messageId },
                    { "Timestamp", DateTime.UtcNow }
                };

                await _publisher.PublishAsync(ExchangeName, routingKey, message, headers);
                _logger.LogInformation($"Domain event published: {eventType} (MessageId: {messageId})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing domain event: {typeof(TEvent).Name}");
                throw;
            }
        }

        public async Task PublishManyAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var @event in events)
                {
                    var eventType = @event.GetType();
                    var publishMethod = typeof(RabbitMQDomainEventPublisher)
                        .GetMethod(nameof(PublishAsync))
                        ?.MakeGenericMethod(eventType);

                    if (publishMethod != null)
                    {
                        await (Task)publishMethod.Invoke(this, new object[] { @event, cancellationToken });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing multiple domain events");
                throw;
            }
        }

        private string GetRoutingKey(string eventTypeName)
        {
            // Convert event type name to routing key format
            // e.g., UserMapCreatedEvent -> user.map.created
            return eventTypeName
                .Replace("Event", "")
                .Replace("UserMap", "user.map")
                .Replace("UserRole", "user.role")
                .ToLower();
        }
    }
}
