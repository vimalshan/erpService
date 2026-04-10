using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ClubMembershipService.Infrastructure.Messaging;

public class RabbitMqPublisher : IAsyncDisposable
{
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _connectionFailed;

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
        };
    }

    private async Task EnsureConnectedAsync()
    {
        if (_channel is not null || _connectionFailed) return;
        try
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync("club_membership_exchange", ExchangeType.Topic, durable: true);
            _logger.LogInformation("RabbitMQ publisher connected to {Host}:{Port}", _factory.HostName, _factory.Port);
        }
        catch (Exception ex)
        {
            _connectionFailed = true;
            _logger.LogWarning(ex, "RabbitMQ publisher connection failed. Publishing will be skipped.");
        }
    }

    public async Task PublishAsync<T>(string routingKey, T message)
    {
        await EnsureConnectedAsync();
        if (_channel is null) return;
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
        await _channel.BasicPublishAsync(
            "club_membership_exchange", routingKey, false, props, body);
        _logger.LogInformation("Published message to {RoutingKey}", routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
