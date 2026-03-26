using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LeaveServices.Infrastructure.Messaging;

public sealed class RabbitMqSettings
{
    public string Host              { get; set; } = "localhost";
    public int    Port              { get; set; } = 5672;
    public string UserName          { get; set; } = "guest";
    public string Password          { get; set; } = "guest";
    public string VHost             { get; set; } = "/";
    public string LeaveAppliedQueue  { get; set; } = "leave.applied";
    public string LeaveApprovedQueue { get; set; } = "leave.approved";
}

public sealed class RabbitMqPublisher : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel    _channel;

    private RabbitMqPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel    = channel;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(RabbitMqSettings settings)
    {
        var factory = new ConnectionFactory
        {
            HostName    = settings.Host,
            Port        = settings.Port,
            UserName    = settings.UserName,
            Password    = settings.Password,
            VirtualHost = settings.VHost
        };
        var conn    = await factory.CreateConnectionAsync();
        var channel = await conn.CreateChannelAsync();
        return new RabbitMqPublisher(conn, channel);
    }

    public async Task PublishAsync(string exchange, string routingKey, object message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
        await _channel.BasicPublishAsync(exchange, routingKey, true, props, body);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}

public sealed class LeaveAppliedConsumer : IAsyncDisposable
{
    private readonly ILogger<LeaveAppliedConsumer> _logger;
    private IConnection? _connection;
    private IChannel?    _channel;

    public LeaveAppliedConsumer(ILogger<LeaveAppliedConsumer> logger) => _logger = logger;

    public async Task StartAsync(RabbitMqSettings settings, CancellationToken ct)
    {
        var factory = new ConnectionFactory
        {
            HostName    = settings.Host,
            Port        = settings.Port,
            UserName    = settings.UserName,
            Password    = settings.Password,
            VirtualHost = settings.VHost
        };
        _connection = await factory.CreateConnectionAsync(ct);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: ct);

        await _channel.QueueDeclareAsync(settings.LeaveAppliedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body    = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("[LeaveApplied] Received: {Message}", message);
            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(settings.LeaveAppliedQueue, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}

public sealed class LeaveApprovedConsumer : IAsyncDisposable
{
    private readonly ILogger<LeaveApprovedConsumer> _logger;
    private IConnection? _connection;
    private IChannel?    _channel;

    public LeaveApprovedConsumer(ILogger<LeaveApprovedConsumer> logger) => _logger = logger;

    public async Task StartAsync(RabbitMqSettings settings, CancellationToken ct)
    {
        var factory = new ConnectionFactory
        {
            HostName    = settings.Host,
            Port        = settings.Port,
            UserName    = settings.UserName,
            Password    = settings.Password,
            VirtualHost = settings.VHost
        };
        _connection = await factory.CreateConnectionAsync(ct);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: ct);

        await _channel.QueueDeclareAsync(settings.LeaveApprovedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body    = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("[LeaveApproved] Received: {Message}", message);
            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(settings.LeaveApprovedQueue, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}
