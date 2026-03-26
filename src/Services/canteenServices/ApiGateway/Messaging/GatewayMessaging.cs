using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiGateway.Messaging;

public class RabbitMQSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "gateway.exchange";
}

public class GatewayEventPublisher : IAsyncDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<GatewayEventPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _available;

    public GatewayEventPublisher(IOptions<RabbitMQSettings> settings, ILogger<GatewayEventPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    private async Task EnsureConnectionAsync()
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true }) return;

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true);
            _available = true;
            _logger.LogInformation("[GatewayPublisher] Connected to RabbitMQ at {Host}:{Port}", _settings.Host, _settings.Port);
        }
        catch (Exception ex)
        {
            _available = false;
            _logger.LogWarning(ex, "[GatewayPublisher] RabbitMQ unavailable — gateway events will not be published");
        }
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        await EnsureConnectionAsync();
        if (!_available || _channel is null) return;

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);

        _logger.LogInformation("[GatewayPublisher] Published {RoutingKey}", routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is { IsOpen: true }) await _channel.CloseAsync();
        if (_connection is { IsOpen: true }) await _connection.CloseAsync();
    }
}

/// <summary>
/// Listens for cross-service events (e.g., service status changes, configuration updates).
/// </summary>
public class GatewayEventConsumer : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<GatewayEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string QueueName = "gateway.events.queue";

    public GatewayEventConsumer(IOptions<RabbitMQSettings> settings, ILogger<GatewayEventConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: ct);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

            // Bind to all cross-service events
            await _channel.QueueBindAsync(QueueName, _settings.ExchangeName, "gateway.#", cancellationToken: ct);
            await _channel.QueueBindAsync(QueueName, _settings.ExchangeName, "service.status.#", cancellationToken: ct);

            _logger.LogInformation("[GatewayConsumer] Connected — listening on {Queue}", QueueName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[GatewayConsumer] RabbitMQ unavailable — consumer will not start");
            return;
        }

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null) return;

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var routingKey = ea.RoutingKey;

                _logger.LogInformation("[GatewayConsumer] Received {RoutingKey}: {Body}",
                    routingKey, body.Length > 200 ? body[..200] + "..." : body);

                // Process gateway-level events here (e.g., configuration reloads, alerts)

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GatewayConsumer] Error processing message");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        // Keep alive
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is { IsOpen: true }) await _channel.CloseAsync(cancellationToken: ct);
        if (_connection is { IsOpen: true }) await _connection.CloseAsync(cancellationToken: ct);
        await base.StopAsync(ct);
    }
}
