using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace MedicalVisit.Infrastructure.Messaging;

public class RabbitMQPublisher : IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    private bool _isAvailable;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeAsync()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _isAvailable = true;
        }
        catch
        {
            _isAvailable = false;
        }
    }

    public void Publish<T>(string exchange, string routingKey, T message)
    {
        if (!_isAvailable || _channel == null) return;

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true)
            .GetAwaiter().GetResult();

        _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body).GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _channel?.DisposeAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
        _connection?.DisposeAsync().GetAwaiter().GetResult();
    }
}
