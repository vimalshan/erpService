using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace ObjectiveService.Infrastructure.Services;

public interface IRabbitMQService
{
    Task PublishMessageAsync<T>(string exchange, string routingKey, T message) where T : class;
    Task SubscribeAsync<T>(string queue, string exchange, string routingKey, Func<T, Task> onMessageReceived) where T : class;
}

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQService> _logger;
    private IChannel _channel;

    public RabbitMQService(IConnection connection, ILogger<RabbitMQService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    private async Task InitializeChannelAsync()
    {
        if (_channel == null)
        {
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation("RabbitMQ channel initialized");
        }
    }

    public async Task PublishMessageAsync<T>(string exchange, string routingKey, T message) where T : class
    {
        try
        {
            await InitializeChannelAsync();
            
            await _channel.ExchangeDeclareAsync(exchange, "topic", durable: true);

            var body = System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));
            var properties = new BasicProperties { Persistent = true };

            await _channel.BasicPublishAsync(exchange, routingKey, false, properties, body);

            _logger.LogInformation("Message published to exchange: {Exchange}, routing key: {RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
            throw;
        }
    }

    public async Task SubscribeAsync<T>(string queue, string exchange, string routingKey, Func<T, Task> onMessageReceived) where T : class
    {
        try
        {
            await InitializeChannelAsync();
            
            await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(queue, exchange, routingKey);

            _logger.LogInformation("Queue {Queue} subscribed to {Exchange} with routing key {RoutingKey}", queue, exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subscribing to RabbitMQ queue");
            throw;
        }
    }
}
