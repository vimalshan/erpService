using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PayrollServices.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message publisher
/// </summary>
public class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "payroll.exchange";

    public RabbitMqPublisher(IConnection connection)
    {
        _connection = connection;
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
    }

    public void PublishMessage<T>(string routingKey, T message)
    {
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(ExchangeName, routingKey, properties, body);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to publish message to RabbitMQ: {ex.Message}", ex);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}
