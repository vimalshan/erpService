using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AccessService.Domain.Events;
using AccessService.Infrastructure.DomainEvents;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ.Consumers
{
    /// <summary>
    /// Consumer for UserMapCreatedEvent domain events
    /// Processes notifications when a new user mapping is created
    /// </summary>
    public class UserMapCreatedEventConsumer : RabbitMQConsumer
    {
        private readonly IDomainEventHandler<UserMapCreatedEvent> _handler;

        protected override string QueueName => "access-service.user-map.created";
        protected override string ExchangeName => "access-service-exchange";
        protected override string RoutingKey => "user.map.created";

        public UserMapCreatedEventConsumer(
            IRabbitMQConnection connection,
            IdempotencyService idempotencyService,
            IDomainEventHandler<UserMapCreatedEvent> handler,
            ILogger<UserMapCreatedEventConsumer> logger)
            : base(connection, idempotencyService, logger)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected override async Task ProcessMessageAsync(string message, string messageId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var @event = JsonSerializer.Deserialize<UserMapCreatedEvent>(message, options);

                if (@event != null)
                {
                    await _handler.HandleAsync(@event);
                    _logger.LogInformation($"UserMapCreatedEvent handled: EmployeeSystemId={@event.EmployeeSystemId}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize UserMapCreatedEvent");
                throw;
            }
        }
    }
}
