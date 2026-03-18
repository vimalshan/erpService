namespace CommunityService.Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey) where T : IDomainEvent;
}

public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "community.events";

    public RabbitMqPublisher(string hostName, string userName = "guest", string password = "guest")
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare exchange
        _channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);
    }

    public async Task PublishAsync<T>(T message, string routingKey) where T : IDomainEvent
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to publish message: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}

public interface IMessageConsumer
{
    Task StartAsync();
    Task StopAsync();
}

public class CommunityEventConsumer : IMessageConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CommunityEventConsumer> _logger;
    private const string ExchangeName = "community.events";

    public CommunityEventConsumer(string hostName, ILogger<CommunityEventConsumer> logger, 
        string userName = "guest", string password = "guest")
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare exchange and queues
        _channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        // Declare queue for community events
        var queueName = _channel.QueueDeclare(
            queue: "community.events.queue",
            durable: true,
            exclusive: false,
            autoDelete: false).QueueName;

        // Bind queue to exchange
        _channel.QueueBind(
            queue: queueName,
            exchange: ExchangeName,
            routingKey: "community.*");

        _channel.BasicQos(0, 1, false);
    }

    public async Task StartAsync()
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {Message}", json);

                // TODO: Process the message based on type
                // Parse and handle the event

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: "community.events.queue", autoAck: false, consumer: consumer);
        await Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _channel?.Close();
        _connection?.Close();
        await Task.CompletedTask;
    }
}
