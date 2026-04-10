using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MobileExpenseManagement.Application.Common.Interfaces;

namespace MobileExpenseManagement.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message bus implementation
/// </summary>
public class RabbitMQMessageBus : IMessageBus
{
    private readonly IRabbitMQConnection _connection;
    private readonly ILogger<RabbitMQMessageBus> _logger;

    public RabbitMQMessageBus(IRabbitMQConnection connection, ILogger<RabbitMQMessageBus> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            using (var channel = _connection.CreateModel())
            {
                var exchangeName = typeof(T).Name;
                var queueName = typeof(T).Name.ToLower();
                
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey ?? queueName);

                var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";

                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey ?? queueName, 
                    basicProperties: properties, body: messageBytes);

                _logger.LogInformation("Message published: {MessageType} to {Exchange}", typeof(T).Name, exchangeName);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable. Message not published: {MessageType}", typeof(T).Name);
        }
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler) where T : class
    {
        try
        {
            using (var channel = _connection.CreateModel())
            {
                var exchangeName = typeof(T).Name;
                
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: queueName);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                        if (message != null)
                        {
                            await handler(message);
                            channel.BasicAck(ea.DeliveryTag, false);
                            _logger.LogInformation("Message processed: {MessageType}", typeof(T).Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message: {MessageType}", typeof(T).Name);
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                _logger.LogInformation("Subscription registered for: {MessageType}", typeof(T).Name);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable. Could not subscribe to: {MessageType}", typeof(T).Name);
        }
    }
}

/// <summary>
/// RabbitMQ connection interface and implementation
/// </summary>
public interface IRabbitMQConnection : IDisposable
{
    IModel CreateModel();
}

public class RabbitMQConnection : IRabbitMQConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQConnection> _logger;
    private IConnection? _connection;

    public RabbitMQConnection(IConnectionFactory connectionFactory, ILogger<RabbitMQConnection> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public IModel CreateModel()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not connect to RabbitMQ. Messaging will be unavailable.");
                throw;
            }
        }
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
