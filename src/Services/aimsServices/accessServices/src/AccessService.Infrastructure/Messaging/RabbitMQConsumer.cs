namespace AccessService.Infrastructure.Messaging;

using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using AccessService.Domain.Events;
using Microsoft.Extensions.Logging;

/// <summary>
/// RabbitMQ domain event consumer for consuming domain events from message broker
/// </summary>
public interface IRabbitMQConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Implementation of RabbitMQ domain event consumer
/// Listens for domain events and processes them
/// </summary>
public class RabbitMQConsumer : IRabbitMQConsumer, IDisposable
{
    private readonly IRabbitMQConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQConsumer> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    private const string EXCHANGE_NAME = "access_service_exchange";
    private const string EXCHANGE_TYPE = "topic";
    private const string QUEUE_NAME = "access_service_queue";
    private const string ROUTING_KEY = "access.#"; // Subscribe to all access events

    public RabbitMQConsumer(IRabbitMQConnectionFactory connectionFactory, ILogger<RabbitMQConsumer> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Starts consuming messages from RabbitMQ
    /// </summary>
    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        try
        {
            EnsureConnection();

            // Declare queue
            _channel!.QueueDeclare(
                queue: QUEUE_NAME,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind queue to exchange
            _channel.QueueBind(
                queue: QUEUE_NAME,
                exchange: EXCHANGE_NAME,
                routingKey: ROUTING_KEY
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => await OnMessageReceivedAsync(ea);

            _channel.BasicConsume(
                queue: QUEUE_NAME,
                autoAck: false,
                consumerTag: "access-service-consumer",
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer
            );

            _logger.LogInformation($"Started consuming messages from queue: {QUEUE_NAME}");

            // Keep listening until cancellation is requested
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error consuming messages: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Handles received messages
    /// </summary>
    private async Task OnMessageReceivedAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            _logger.LogInformation($"Message received - Routing Key: {routingKey}, Message: {message}");

            // Route message based on event type
            await ProcessEventAsync(routingKey, message);

            // Acknowledge successful processing
            _channel!.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            // Nack and requeue on error
            _channel!.BasicNack(ea.DeliveryTag, false, true);
        }
    }

    /// <summary>
    /// Routes event messages to appropriate handlers
    /// </summary>
    private async Task ProcessEventAsync(string routingKey, string message)
    {
        try
        {
            // Example: access.usermapcreatdevent → UserMapCreatedEvent
            if (routingKey.Contains("usermapcreated", StringComparison.OrdinalIgnoreCase))
            {
                var @event = JsonConvert.DeserializeObject<UserMapCreatedEvent>(message);
                if (@event != null)
                {
                    _logger.LogInformation($"Processing UserMapCreatedEvent for employee: {@event.EmployeeSystemId}");
                    // TODO: Call appropriate handler or service
                }
            }
            else if (routingKey.Contains("usermapactivated", StringComparison.OrdinalIgnoreCase))
            {
                var @event = JsonConvert.DeserializeObject<UserMapActivatedEvent>(message);
                if (@event != null)
                {
                    _logger.LogInformation($"Processing UserMapActivatedEvent for employee: {@event.EmployeeSystemId}");
                    // TODO: Call appropriate handler or service
                }
            }
            else if (routingKey.Contains("userroleassigned", StringComparison.OrdinalIgnoreCase))
            {
                var @event = JsonConvert.DeserializeObject<UserRoleAssignedEvent>(message);
                if (@event != null)
                {
                    _logger.LogInformation($"Processing UserRoleAssignedEvent for role: {@event.RoleId}");
                    // TODO: Call appropriate handler or service
                }
            }
            else if (routingKey.Contains("userrolerevo", StringComparison.OrdinalIgnoreCase))
            {
                var @event = JsonConvert.DeserializeObject<UserRoleRevokedEvent>(message);
                if (@event != null)
                {
                    _logger.LogInformation($"Processing UserRoleRevokedEvent for role: {@event.RoleId}");
                    // TODO: Call appropriate handler or service
                }
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing event from routing key '{routingKey}': {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Ensures connection and channel are established
    /// </summary>
    private void EnsureConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        if (_channel == null || _channel.IsClosed)
        {
            _channel = _connectionFactory.CreateChannel(_connection);
            
            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: EXCHANGE_TYPE,
                durable: true,
                autoDelete: false,
                arguments: null
            );
        }
    }

    /// <summary>
    /// Closes the connection and channel
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        _logger.LogInformation("RabbitMQ consumer connection and channel closed");
    }
}
