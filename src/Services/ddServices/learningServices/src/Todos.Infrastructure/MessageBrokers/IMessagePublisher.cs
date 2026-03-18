using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Todos.Infrastructure.MessageBrokers;

/// <summary>
/// Interface for RabbitMQ message publisher
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to a queue
    /// </summary>
    Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Publishes a message to an exchange
    /// </summary>
    Task PublishToExchangeAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// RabbitMQ message publisher implementation
/// </summary>
public class RabbitMQPublisher : IMessagePublisher
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQPublisher> _logger;

    public RabbitMQPublisher(IConnectionFactory connectionFactory, ILogger<RabbitMQPublisher> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        try
        {
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var json = System.Text.Json.JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            _logger.LogInformation("Message published to queue {QueueName}", queueName);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to queue {QueueName}", queueName);
            throw;
        }
    }

    public async Task PublishToExchangeAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        try
        {
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, durable: true);

            var json = System.Text.Json.JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: properties, body: body);
            _logger.LogInformation("Message published to exchange {Exchange} with routing key {RoutingKey}", exchange, routingKey);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to exchange {Exchange}", exchange);
            throw;
        }
    }
}
