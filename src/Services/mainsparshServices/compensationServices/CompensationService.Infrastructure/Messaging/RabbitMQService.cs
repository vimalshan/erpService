using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CompensationService.Infrastructure.Messaging;

/// <summary>
/// Interface for RabbitMQ messaging
/// </summary>
public interface IRabbitMQService
{
    Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
    void SubscribeToQueue(string queueName, Func<string, Task> onMessageReceived);
}

/// <summary>
/// Implementation of RabbitMQ Service
/// </summary>
public class RabbitMQService : IRabbitMQService
{
    private readonly IConnection? _connection;
    private readonly IModel? _channel;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IConnection? connection, ILogger<RabbitMQService> logger)
    {
        _connection = connection;
        _channel = _connection?.CreateModel();
        _logger = logger;

        if (_connection == null)
        {
            _logger.LogWarning("RabbitMQ connection is not available. Messaging features are disabled.");
        }
    }

    public async Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        if (_channel == null)
        {
            _logger.LogWarning("RabbitMQ is not available. Message to {Exchange}/{RoutingKey} was not published.", exchange, routingKey);
            await Task.CompletedTask;
            return;
        }

        try
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(serializedMessage);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(exchange, routingKey, properties, body);
            _logger.LogInformation($"Message published to {exchange}/{routingKey}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error publishing message: {ex.Message}");
            throw;
        }
    }

    public void SubscribeToQueue(string queueName, Func<string, Task> onMessageReceived)
    {
        if (_channel == null)
        {
            _logger.LogWarning("RabbitMQ is not available. Cannot subscribe to queue {QueueName}.", queueName);
            return;
        }

        try
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            
            var consumer = new RabbitMQ.Client.Events.AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                
                try
                {
                    await onMessageReceived(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation($"Message processed from {queueName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation($"Subscribed to queue: {queueName}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error subscribing to queue: {ex.Message}");
            throw;
        }
    }
}
