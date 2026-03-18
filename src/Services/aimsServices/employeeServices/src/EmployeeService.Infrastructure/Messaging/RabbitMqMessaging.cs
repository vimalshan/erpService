using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace EmployeeService.Infrastructure.Messaging;

/// <summary>Publishes domain events to RabbitMQ exchanges.</summary>
public sealed class RabbitMqPublisher : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConnectionFactory factory, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(T message, string exchangeName, string routingKey = "", CancellationToken ct = default)
    {
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, durable: true, cancellationToken: ct);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };

        await _channel.BasicPublishAsync(exchangeName, routingKey, false, props, body, ct);
        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchangeName, routingKey);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().Wait();
        _connection?.CloseAsync().Wait();
    }
}

/// <summary>Base consumer for RabbitMQ messages.</summary>
public abstract class RabbitMqConsumerBase : IDisposable
{
    private readonly IConnection _connection;
    protected readonly IChannel Channel;
    protected readonly ILogger Logger;

    protected RabbitMqConsumerBase(IConnectionFactory factory, ILogger logger)
    {
        Logger = logger;
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        Channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    protected abstract Task HandleMessageAsync(string message, CancellationToken ct);

    protected async Task StartConsumingAsync(string queueName, CancellationToken ct = default)
    {
        await Channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        var consumer = new AsyncEventingBasicConsumer(Channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.Span);
            try
            {
                await HandleMessageAsync(body, ct);
                await Channel.BasicAckAsync(ea.DeliveryTag, false, ct);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing message from {Queue}", queueName);
                await Channel.BasicNackAsync(ea.DeliveryTag, false, false, ct);
            }
        };

        await Channel.BasicConsumeAsync(queueName, false, consumer, ct);
    }

    public void Dispose()
    {
        Channel?.CloseAsync().Wait();
        _connection?.CloseAsync().Wait();
    }
}

/// <summary>Specific consumer for attendance flag change events.</summary>
public sealed class AttendanceFlagConsumer : RabbitMqConsumerBase
{
    public AttendanceFlagConsumer(IConnectionFactory factory, ILogger<AttendanceFlagConsumer> logger)
        : base(factory, logger) { }

    public Task StartAsync(CancellationToken ct) =>
        StartConsumingAsync("employee.attendance.updates", ct);

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        Logger.LogInformation("Processing attendance update message: {Message}", message);
        // Deserialize and process attendance update logic here
        return Task.CompletedTask;
    }
}

/// <summary>Consumer for approver assignment events.</summary>
public sealed class ApproverAssignmentConsumer : RabbitMqConsumerBase
{
    public ApproverAssignmentConsumer(IConnectionFactory factory, ILogger<ApproverAssignmentConsumer> logger)
        : base(factory, logger) { }

    public Task StartAsync(CancellationToken ct) =>
        StartConsumingAsync("employee.approver.assignments", ct);

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        Logger.LogInformation("Processing approver assignment message: {Message}", message);
        return Task.CompletedTask;
    }
}
