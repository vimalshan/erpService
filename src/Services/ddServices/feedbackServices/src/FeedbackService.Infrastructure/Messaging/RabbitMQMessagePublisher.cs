namespace FeedbackService.Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

/// <summary>
/// RabbitMQ message publisher implementation
/// </summary>
public class RabbitMQMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "feedback.events";

    /// <summary>
    /// Initializes a new instance of the RabbitMQMessagePublisher class
    /// </summary>
    public RabbitMQMessagePublisher(IConnection connection)
    {
        _connection = connection;
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            arguments: null);
    }

    /// <summary>
    /// Publishes a message to RabbitMQ
    /// </summary>
    public async Task PublishAsync(object message, CancellationToken cancellationToken = default)
    {
        var messageType = message.GetType().Name;
        var routingKey = $"feedback.{messageType.ToLowerInvariant()}";
        
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = _channel.CreateBasicProperties();
        properties.ContentType = "application/json";
        properties.DeliveryMode = 2; // Persistent
        properties.Headers = new Dictionary<string, object>
        {
            { "MessageType", messageType },
            { "TimeSent", DateTime.UtcNow }
        };

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the RabbitMQ connection
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        _channel?.Dispose();
        await Task.CompletedTask;
    }
}
