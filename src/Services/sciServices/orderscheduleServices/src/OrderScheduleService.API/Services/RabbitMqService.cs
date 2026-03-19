namespace OrderScheduleService.API.Services;

using RabbitMQ.Client;
using OrderScheduleService.IntegrationEvents;
using System.Text.Json;
using System.Text;

public interface IRabbitMqPublisher
{
    Task PublishEventAsync<T>(T integrationEvent) where T : IntegrationEvent;
}

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IConnection _connection;
    private readonly OrderScheduleService.IntegrationEvents.RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(
        OrderScheduleService.IntegrationEvents.RabbitMqConfiguration config,
        ILogger<RabbitMqPublisher> logger)
    {
        _config = config;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password,
            VirtualHost = _config.VirtualHost
        };

        try
        {
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ connection");
            throw;
        }
    }

    public async Task PublishEventAsync<T>(T integrationEvent) where T : IntegrationEvent
    {
        try
        {
            await using var channel = await _connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _config.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            await channel.QueueDeclareAsync(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: _config.QueueName,
                exchange: _config.ExchangeName,
                routingKey: typeof(T).Name);

            var message = JsonSerializer.Serialize(integrationEvent);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await channel.BasicPublishAsync(
                exchange: _config.ExchangeName,
                routingKey: typeof(T).Name,
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogInformation($"Published event: {typeof(T).Name} with ID: {integrationEvent.EventId}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to publish event: {typeof(T).Name}");
            throw;
        }
    }
}

public interface IRabbitMqConsumer
{
    void StartConsuming();
}

public class RabbitMqConsumer : IRabbitMqConsumer
{
    private readonly IConnection _connection;
    private readonly OrderScheduleService.IntegrationEvents.RabbitMqConfiguration _config;
    private readonly ILogger<RabbitMqConsumer> _logger;

    public RabbitMqConsumer(
        OrderScheduleService.IntegrationEvents.RabbitMqConfiguration config,
        ILogger<RabbitMqConsumer> logger)
    {
        _config = config;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password,
            VirtualHost = _config.VirtualHost
        };

        try
        {
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ connection");
            throw;
        }
    }

    public void StartConsuming()
    {
        try
        {
            var channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            channel.ExchangeDeclareAsync(
                exchange: _config.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false).GetAwaiter().GetResult();

            channel.QueueDeclareAsync(
                queue: _config.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false).GetAwaiter().GetResult();

            channel.QueueBindAsync(
                queue: _config.QueueName,
                exchange: _config.ExchangeName,
                routingKey: "#").GetAwaiter().GetResult();

            _logger.LogInformation("RabbitMQ consumer started and consuming messages...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start consuming messages");
            throw;
        }
    }
}
