using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TourPlanService.Application.Interfaces;

namespace TourPlanService.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqOptions
{
    public const string Section = "RabbitMQ";
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "tourplan_exchange";
}

public sealed class RabbitMqPublisher(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqPublisher> logger) : IMessagePublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMqOptions _options = options.Value;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private DateTime _lastFailedAttempt = DateTime.MinValue;
    private static readonly TimeSpan RetryCooldown = TimeSpan.FromSeconds(30);

    private async Task<IChannel?> GetChannelAsync(CancellationToken ct)
    {
        if (_channel is not null && _connection?.IsOpen == true) return _channel;

        // Cooldown: don't spam connection attempts on every publish call
        if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown)
            return null;

        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is not null && _connection?.IsOpen == true) return _channel;
            if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown) return null;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: ct);
            _lastFailedAttempt = DateTime.MinValue; // Reset on success
            return _channel;
        }
        catch (Exception ex)
        {
            _lastFailedAttempt = DateTime.UtcNow;
            logger.LogWarning(ex, "Failed to connect to RabbitMQ at {Host}:{Port}. Messages will be skipped for {Cooldown}s.",
                _options.HostName, _options.Port, RetryCooldown.TotalSeconds);
            return null;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PublishAsync<T>(string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default)
        where T : class
    {
        var channel = await GetChannelAsync(cancellationToken);
        if (channel is null)
        {
            logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchangeName, routingKey);
            return;
        }

        try
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(message);
            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken);

            logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchangeName, routingKey);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to publish to {Exchange}/{RoutingKey}. Resetting connection.", exchangeName, routingKey);
            await ResetConnectionAsync();
        }
    }

    private async Task ResetConnectionAsync()
    {
        await _lock.WaitAsync();
        try
        {
            try { if (_channel is not null) await _channel.CloseAsync(); } catch { /* best-effort */ }
            try { if (_connection is not null) await _connection.CloseAsync(); } catch { /* best-effort */ }
            _channel = null;
            _connection = null;
            _lastFailedAttempt = DateTime.UtcNow;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        try { if (_channel is not null) await _channel.CloseAsync(); } catch { /* best-effort */ }
        try { if (_connection is not null) await _connection.CloseAsync(); } catch { /* best-effort */ }
        GC.SuppressFinalize(this);
    }
}
