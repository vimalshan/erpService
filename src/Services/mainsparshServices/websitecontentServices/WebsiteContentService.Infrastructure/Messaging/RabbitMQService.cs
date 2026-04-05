namespace WebsiteContentService.Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

public interface IRabbitMQService
{
    Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection? _connection;
    private readonly IChannel? _channel;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IConnection? connection, ILogger<RabbitMQService> logger)
    {
        _connection = connection;
        _logger = logger;

        if (_connection is null)
        {
            _logger.LogWarning("RabbitMQ connection is not available. Messaging features are disabled.");
            return;
        }

        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        if (_channel is null)
        {
            _logger.LogWarning("RabbitMQ is not available. Message to {Exchange}/{RoutingKey} was not published.", exchange, routingKey);
            return;
        }

        try
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await _channel.BasicPublishAsync(exchange, routingKey, false, properties, body, ct);
            _logger.LogInformation("Message published to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to {Exchange}/{RoutingKey}", exchange, routingKey);
            throw;
        }
    }
}
