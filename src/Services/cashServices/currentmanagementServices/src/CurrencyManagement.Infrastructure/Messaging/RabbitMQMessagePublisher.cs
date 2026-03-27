using CurrencyManagement.Application.Common.Interfaces;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace CurrencyManagement.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message publisher for domain events and integration events
/// </summary>
public class RabbitMQMessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQMessagePublisher> _logger;

    public RabbitMQMessagePublisher(IConnection connection, ILogger<RabbitMQMessagePublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            using (var channel = _connection.CreateModel())
            {
                var exchangeName = "currency_management";
                var routingKey = typeof(T).Name;

                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                var messageJson = JsonSerializer.Serialize(message);
                var body = System.Text.Encoding.UTF8.GetBytes(messageJson);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";

                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation($"Published message of type {typeof(T).Name} with routing key {routingKey}");
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable - message of type {MessageType} not published (graceful degradation)", typeof(T).Name);
        }
    }
}
