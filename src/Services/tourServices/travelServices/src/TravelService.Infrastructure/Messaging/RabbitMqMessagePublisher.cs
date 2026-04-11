using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TravelService.Application.Common.Interfaces;

namespace TravelService.Infrastructure.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;
    private DateTime _lastFailedAttempt = DateTime.MinValue;
    private static readonly TimeSpan RetryCooldown = TimeSpan.FromSeconds(30);

    public RabbitMqMessagePublisher(IConfiguration configuration, ILogger<RabbitMqMessagePublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private async Task<IChannel?> GetOrCreateChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel is { IsOpen: true })
            return _channel;

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (_channel is { IsOpen: true })
                return _channel;

            if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown)
                return null;

            // Cleanup stale connection
            if (_channel is not null) { try { await _channel.CloseAsync(cancellationToken); } catch { } _channel = null; }
            if (_connection is not null) { try { await _connection.CloseAsync(cancellationToken); } catch { } _connection = null; }

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/"
            };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("RabbitMQ connection established for publishing");
            return _channel;
        }
        catch (Exception ex)
        {
            _lastFailedAttempt = DateTime.UtcNow;
            _logger.LogWarning(ex, "RabbitMQ not available, will retry after {Cooldown}s", RetryCooldown.TotalSeconds);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message,
        CancellationToken cancellationToken = default)
    {
        var channel = await GetOrCreateChannelAsync(cancellationToken);
        if (channel is null)
        {
            _logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        try
        {
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
            await channel.BasicPublishAsync(exchange, routingKey, false, props, body, cancellationToken);
            _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) { try { await _channel.CloseAsync(); } catch { } }
        if (_connection is not null) { try { await _connection.CloseAsync(); } catch { } }
        _semaphore.Dispose();
    }
}
