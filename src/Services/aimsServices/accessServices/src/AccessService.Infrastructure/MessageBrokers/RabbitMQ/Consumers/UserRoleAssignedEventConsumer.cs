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
    /// Consumer for UserRoleAssignedEvent domain events
    /// Processes notifications when a role is assigned to a user
    /// </summary>
    public class UserRoleAssignedEventConsumer : RabbitMQConsumer
    {
        private readonly IDomainEventHandler<UserRoleAssignedEvent> _handler;

        protected override string QueueName => "access-service.user-role.assigned";
        protected override string ExchangeName => "access-service-exchange";
        protected override string RoutingKey => "user.role.assigned";

        public UserRoleAssignedEventConsumer(
            IRabbitMQConnection connection,
            IdempotencyService idempotencyService,
            IDomainEventHandler<UserRoleAssignedEvent> handler,
            ILogger<UserRoleAssignedEventConsumer> logger)
            : base(connection, idempotencyService, logger)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected override async Task ProcessMessageAsync(string message, string messageId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var @event = JsonSerializer.Deserialize<UserRoleAssignedEvent>(message, options);

                if (@event != null)
                {
                    await _handler.HandleAsync(@event);
                    _logger.LogInformation($"UserRoleAssignedEvent handled: EmployeeSystemId={@event.EmployeeSystemId}, RoleId={@event.RoleId}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize UserRoleAssignedEvent");
                throw;
            }
        }
    }
}
