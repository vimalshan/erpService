using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TransactionService.Infrastructure.Messaging;

public sealed class MessagePublisher : IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessagePublisher> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;
    private DateTime _lastFailedAttempt = DateTime.MinValue;
    private static readonly TimeSpan RetryCooldown = TimeSpan.FromSeconds(30);

    public MessagePublisher(IConfiguration configuration, ILogger<MessagePublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private async Task<IChannel?> GetChannelAsync(CancellationToken ct)
    {
        if (_channel is not null && _connection?.IsOpen == true) return _channel;
        if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown) return null;

        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is not null && _connection?.IsOpen == true) return _channel;
            if (DateTime.UtcNow - _lastFailedAttempt < RetryCooldown) return null;

            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/",
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            _lastFailedAttempt = DateTime.MinValue;
            return _channel;
        }
        catch (Exception ex)
        {
            _lastFailedAttempt = DateTime.UtcNow;
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ. Messages will be skipped for {Cooldown}s.",
                RetryCooldown.TotalSeconds);
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
            _logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                body: body,
                cancellationToken: ct);

            _logger.LogInformation("Published to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish to {Exchange}/{RoutingKey}. Resetting connection.", exchange, routingKey);
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
