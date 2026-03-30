using CompensationBenefits.Application.Contracts;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CompensationBenefits.Infrastructure.Messaging;

public class RabbitMqMessagePublisher(IConnection connection, ILogger<RabbitMqMessagePublisher> logger)
    : IMessagePublisher, IAsyncDisposable
{
    private IChannel? _channel;

    private async Task<IChannel?> GetChannelAsync()
    {
        if (connection is null) return null;
        _channel ??= await connection.CreateChannelAsync();
        return _channel;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        var channel = await GetChannelAsync();
        if (channel is null)
        {
            logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }
        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(exchange, routingKey, mandatory: false, props, body);
        logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
    }
}

/// <summary>Consumer that listens for salary-processing events on RabbitMQ.</summary>
public class SalaryEventConsumer(IConnection connection, ILogger<SalaryEventConsumer> logger)
    : Microsoft.Extensions.Hosting.IHostedService, IAsyncDisposable
{
    private IChannel? _channel;

    public async Task StartAsync(CancellationToken ct)
    {
        if (connection is null)
        {
            logger.LogWarning("RabbitMQ unavailable — SalaryEventConsumer not started.");
            return;
        }
        _channel = await connection.CreateChannelAsync(cancellationToken: ct);
        const string exchange = "compensation.events";
        const string queue = "compensation.salary.events";
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        await _channel.QueueBindAsync(queue, exchange, "salary.created", cancellationToken: ct);
        await _channel.QueueBindAsync(queue, exchange, "salary-structure.created", cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.Span);
                logger.LogInformation("Received salary event [{RoutingKey}]: {Body}", ea.RoutingKey, body);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing salary event");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: ct);
        logger.LogInformation("SalaryEventConsumer started — listening on queue '{Queue}'", queue);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
    }
}

/// <summary>Consumer for Mediclaim policy events.</summary>
public class MediclaimEventConsumer(IConnection connection, ILogger<MediclaimEventConsumer> logger)
    : Microsoft.Extensions.Hosting.IHostedService, IAsyncDisposable
{
    private IChannel? _channel;

    public async Task StartAsync(CancellationToken ct)
    {
        if (connection is null)
        {
            logger.LogWarning("RabbitMQ unavailable — MediclaimEventConsumer not started.");
            return;
        }
        _channel = await connection.CreateChannelAsync(cancellationToken: ct);
        const string exchange = "compensation.events";
        const string queue = "compensation.mediclaim.events";
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
        await _channel.QueueBindAsync(queue, exchange, "mediclaim.updated", cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.Span);
                logger.LogInformation("Received mediclaim event [{RoutingKey}]: {Body}", ea.RoutingKey, body);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing mediclaim event");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: ct);
        logger.LogInformation("MediclaimEventConsumer started — listening on queue '{Queue}'", queue);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
    }
}

/// <summary>Processes salary event messages received by the Azure Function trigger.</summary>
public interface ISalaryEventProcessor
{
    Task ProcessAsync(string messageBody, CancellationToken ct);
}

public class SalaryEventProcessor(ILogger<SalaryEventProcessor> logger) : ISalaryEventProcessor
{
    public Task ProcessAsync(string messageBody, CancellationToken ct)
    {
        logger.LogInformation("Processing salary event message. Body length={Len}", messageBody.Length);
        // Deserialize and handle the event payload here.
        // E.g.: var evt = JsonSerializer.Deserialize<SalaryCreatedEvent>(messageBody);
        return Task.CompletedTask;
    }
}
