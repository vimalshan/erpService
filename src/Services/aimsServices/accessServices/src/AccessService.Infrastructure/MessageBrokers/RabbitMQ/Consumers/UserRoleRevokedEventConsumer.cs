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
    /// Consumer for UserRoleRevokedEvent domain events
    /// Processes notifications when a role is revoked from a user
    /// </summary>
    public class UserRoleRevokedEventConsumer : RabbitMQConsumer
    {
        private readonly IDomainEventHandler<UserRoleRevokedEvent> _handler;

        protected override string QueueName => "access-service.user-role.revoked";
        protected override string ExchangeName => "access-service-exchange";
        protected override string RoutingKey => "user.role.revoked";

        public UserRoleRevokedEventConsumer(
            IRabbitMQConnection connection,
            IdempotencyService idempotencyService,
            IDomainEventHandler<UserRoleRevokedEvent> handler,
            ILogger<UserRoleRevokedEventConsumer> logger)
            : base(connection, idempotencyService, logger)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        protected override async Task ProcessMessageAsync(string message, string messageId)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var @event = JsonSerializer.Deserialize<UserRoleRevokedEvent>(message, options);

                if (@event != null)
                {
                    await _handler.HandleAsync(@event);
                    _logger.LogInformation($"UserRoleRevokedEvent handled: EmployeeSystemId={@event.EmployeeSystemId}, RoleId={@event.RoleId}");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize UserRoleRevokedEvent");
                throw;
            }
        }
    }
}
