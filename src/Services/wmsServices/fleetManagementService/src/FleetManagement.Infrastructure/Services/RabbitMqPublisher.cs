using System.Text;
using System.Text.Json;
using FleetManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FleetManagement.Infrastructure.Services;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _unavailable;

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private async Task<IChannel?> GetChannelAsync(CancellationToken ct)
    {
        if (_unavailable) return null;
        if (_channel is not null) return _channel;

        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is not null) return _channel;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.TryParse(_configuration["RabbitMQ:Port"], out var p) ? p : 5672
            };
            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            _logger.LogInformation("RabbitMQ connection established");
            return _channel;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ is not available. Messages will not be published");
            _unavailable = true;
            return null;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        var channel = await GetChannelAsync(ct);
        if (channel is null)
        {
            _logger.LogDebug("Skipping publish to {Exchange}/{RoutingKey} — RabbitMQ unavailable", exchange, routingKey);
            return;
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };
        await channel.BasicPublishAsync(exchange, routingKey, mandatory: false, basicProperties: props, body: body, cancellationToken: ct);
        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) { await _channel.CloseAsync(); _channel.Dispose(); }
        if (_connection is not null) { await _connection.CloseAsync(); _connection.Dispose(); }
    }
}
