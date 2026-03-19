using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using BatchAndEnvelopeService.Infrastructure.Settings;

namespace BatchAndEnvelopeService.Infrastructure.Messaging;

public class RabbitMQPublisher : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly RabbitMQSettings _settings;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> options, ILogger<RabbitMQPublisher> logger)
    {
        _settings = options.Value;
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string exchangeName, string routingKey, T message)
    {
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
        await _channel.BasicPublishAsync(exchangeName, routingKey, mandatory: false, basicProperties: props, body: body);
        _logger.LogInformation("[RabbitMQ] Published {MessageType} to {Exchange}/{RoutingKey}", typeof(T).Name, exchangeName, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
