using System.Text;
using System.Text.Json;
using LeaveServices.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LeaveServices.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : IDomainEvent;
}

public sealed class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _initialized;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized) return;

        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _options.LeaveExchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        await _channel.QueueDeclareAsync(_options.EncashmentQueue, durable: true, exclusive: false, autoDelete: false);
        await _channel.QueueDeclareAsync(_options.LopQueue, durable: true, exclusive: false, autoDelete: false);

        await _channel.QueueBindAsync(_options.EncashmentQueue, _options.LeaveExchange, "leave.encashment.*");
        await _channel.QueueBindAsync(_options.LopQueue, _options.LeaveExchange, "leave.lop.*");

        _initialized = true;
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct) where T : IDomainEvent
    {
        try
        {
            await EnsureInitializedAsync();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true, ContentType = "application/json" };

            await _channel!.BasicPublishAsync(
                exchange: _options.LeaveExchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: ct);

            _logger.LogInformation("Published event {EventType} to {Exchange}/{RoutingKey}",
                typeof(T).Name, _options.LeaveExchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventType}", typeof(T).Name);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}

// Domain Event notification handlers that publish to RabbitMQ
public sealed class EncashmentRequestedNotificationHandler : INotificationHandler<LeaveEncashmentRequestedEvent>
{
    private readonly IMessagePublisher _publisher;
    public EncashmentRequestedNotificationHandler(IMessagePublisher publisher) => _publisher = publisher;

    public Task Handle(LeaveEncashmentRequestedEvent notification, CancellationToken ct) =>
        _publisher.PublishAsync(notification, "leave.encashment.requested", ct);
}

public sealed class EncashmentStatusChangedNotificationHandler : INotificationHandler<LeaveEncashmentStatusChangedEvent>
{
    private readonly IMessagePublisher _publisher;
    public EncashmentStatusChangedNotificationHandler(IMessagePublisher publisher) => _publisher = publisher;

    public Task Handle(LeaveEncashmentStatusChangedEvent notification, CancellationToken ct) =>
        _publisher.PublishAsync(notification, "leave.encashment.status_changed", ct);
}

public sealed class LossOfPayNotificationHandler : INotificationHandler<LossOfPayRecordedEvent>
{
    private readonly IMessagePublisher _publisher;
    public LossOfPayNotificationHandler(IMessagePublisher publisher) => _publisher = publisher;

    public Task Handle(LossOfPayRecordedEvent notification, CancellationToken ct) =>
        _publisher.PublishAsync(notification, "leave.lop.recorded", ct);
}
