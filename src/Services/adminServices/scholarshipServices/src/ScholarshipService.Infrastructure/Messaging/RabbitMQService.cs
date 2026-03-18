using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ScholarshipService.Infrastructure.Messaging;

public class RabbitMQSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "scholarship.events";
}

public interface IScholarshipEventPublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default);
}

public class ScholarshipEventPublisher(
    IOptions<RabbitMQSettings> settings,
    ILogger<ScholarshipEventPublisher> logger)
    : IScholarshipEventPublisher, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMQSettings _settings = settings.Value;

    private async Task EnsureConnectionAsync()
    {
        if (_connection is { IsOpen: true }) return;

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true);
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnectionAsync();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await _channel!.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                body: body,
                cancellationToken: cancellationToken);
            logger.LogInformation("Published event {RoutingKey}", routingKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish event {RoutingKey}", routingKey);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
