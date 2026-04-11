using System.Text;
using System.Text.Json;
using BookingService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BookingService.Infrastructure.Messaging;

public class RabbitMqPublisher(string hostName, string userName, string password, ILogger<RabbitMqPublisher> logger) : IMessagePublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private DateTime _lastFailedAttempt = DateTime.MinValue;
    private static readonly TimeSpan RetryCooldown = TimeSpan.FromSeconds(30);

    private async Task<IChannel?> GetChannelAsync(CancellationToken ct)
    {
        if (_channel is not null) return _channel;

        // Cooldown: don't spam connection attempts on every publish call
        if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown)
            return null;

        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is not null) return _channel;
            if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown) return null;

            var factory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password };
            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            _lastFailedAttempt = DateTime.MinValue; // Reset on success
            return _channel;
        }
        catch (Exception ex)
        {
            _lastFailedAttempt = DateTime.UtcNow;
            logger.LogWarning(ex, "Failed to connect to RabbitMQ at {Host}. Messages will be skipped for {Cooldown}s.", hostName, RetryCooldown.TotalSeconds);
            return null;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PublishAsync<T>(T message, string exchangeName, string routingKey, CancellationToken ct = default) where T : class
    {
        var channel = await GetChannelAsync(ct);
        if (channel is null)
        {
            logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchangeName, routingKey);
            return;
        }

        try
        {
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic, durable: true, cancellationToken: ct);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: props, body: body, cancellationToken: ct);
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
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
        GC.SuppressFinalize(this);
    }
}
