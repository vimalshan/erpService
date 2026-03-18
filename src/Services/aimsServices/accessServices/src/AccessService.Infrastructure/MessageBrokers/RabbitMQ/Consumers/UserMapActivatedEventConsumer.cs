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
    /// Consumer for UserMapActivatedEvent domain events
    /// Processes notifications when a user mapping is activated
    /// </summary>
    public class UserMapActivatedEventConsumer : RabbitMQConsumer
    {
        private readonly IDomainEventHandler<UserMapActivatedEvent> _handler;

        protected override string QueueName => "access-service.user-map.activated";
        protected override string ExchangeName => "access-service-exchange";
        protected override string RoutingKey => "user.map.activated";

        public UserMapActivatedEventConsumer(
            IRabbitMQConnection connection,
            IdempotencyService idempotencyService,
            IDomainEventHandler<UserMapActivatedEvent> handler,
            ILogger<UserMapActivatedEventConsumer> logger)
            : base(connection, idempotencyService, logger)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected override async Task ProcessMessageAsync(string message, string messageId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var @event = JsonSerializer.Deserialize<UserMapActivatedEvent>(message, options);

                if (@event != null)
                {
                    await _handler.HandleAsync(@event);
                    _logger.LogInformation($"UserMapActivatedEvent handled: EmployeeSystemId={@event.EmployeeSystemId}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize UserMapActivatedEvent");
                throw;
            }
        }
    }
}
