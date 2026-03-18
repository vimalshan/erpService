using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppraisalService.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message publisher
/// </summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
}

/// <summary>
/// RabbitMQ implementation of message publisher
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

    public async Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true, autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            channel.BasicPublish(exchange, routingKey, properties, body);
            _logger.LogInformation($"Message published to {exchange}/{routingKey}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error publishing message to {exchange}/{routingKey}");
            throw;
        }
    }
}

/// <summary>
/// RabbitMQ message consumer interface
/// </summary>
public interface IMessageConsumer
{
    Task StartConsumingAsync(string queue, string exchange, string routingKey, CancellationToken cancellationToken = default);
    event EventHandler<string>? MessageReceived;
}

/// <summary>
/// RabbitMQ consumer for domain events
/// </summary>
public class DomainEventConsumer : IAsyncDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<DomainEventConsumer> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    public DomainEventConsumer(IConnectionFactory connectionFactory, ILogger<DomainEventConsumer> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task StartAsync(string queue, string exchange, string routingKey)
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue, exchange, routingKey);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Message received: {message}");

                _channel.BasicAck(ea.DeliveryTag, false);

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue, false, consumer);
            _logger.LogInformation($"Started consuming from {queue}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting consumer");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
        }

        await Task.CompletedTask;
    }
}
