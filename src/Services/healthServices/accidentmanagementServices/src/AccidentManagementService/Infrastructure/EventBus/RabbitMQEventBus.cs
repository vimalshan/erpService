using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AccidentManagementService.Infrastructure.EventBus;

public class RabbitMQOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "health_exchange";
    public string QueueName { get; set; } = "accident_queue";
    public string RoutingKey { get; set; } = "accident.*";
}

public class RabbitMQEventBus : IEventBus, IDisposable
{
    private readonly RabbitMQOptions _options;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMQEventBus(IOptions<RabbitMQOptions> options, ILogger<RabbitMQEventBus> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    private async Task EnsureConnectionAsync()
    {
        try
        {
            if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _options.Host,
                    Port = _options.Port,
                    UserName = _options.Username,
                    Password = _options.Password,
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Declare exchange
                _channel.ExchangeDeclare(
                    exchange: _options.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true);

                // Declare queue
                _channel.QueueDeclare(
                    queue: _options.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                // Bind queue to exchange
                _channel.QueueBind(
                    queue: _options.QueueName,
                    exchange: _options.ExchangeName,
                    routingKey: _options.RoutingKey);

                _logger.LogInformation("RabbitMQ connection established");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error establishing RabbitMQ connection");
            throw;
        }
    }

    public async Task PublishAsync(object eventData, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnectionAsync();

            if (_channel == null || !_channel.IsOpen)
            {
                throw new InvalidOperationException("Channel is not open");
            }

            var message = JsonSerializer.Serialize(eventData);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: _options.RoutingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Event published: {EventType}", eventData.GetType().Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event");
            throw;
        }
    }

    public void Dispose()
    {
        try
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing RabbitMQ connection");
        }
    }
}

public interface IEventBus
{
    Task PublishAsync(object eventData, CancellationToken cancellationToken = default);
}
