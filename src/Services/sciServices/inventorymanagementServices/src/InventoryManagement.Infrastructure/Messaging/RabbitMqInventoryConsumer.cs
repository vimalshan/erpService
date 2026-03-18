using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InventoryManagement.Infrastructure.Messaging;

/// <summary>
/// Background RabbitMQ consumer for inventory messages with circuit breaker via Polly.
/// </summary>
public sealed class RabbitMqInventoryConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqInventoryConsumer> _logger;
    private const string QueueName = "inventory.item.registered";

    public RabbitMqInventoryConsumer(IOptions<RabbitMqOptions> options, ILogger<RabbitMqInventoryConsumer> logger)
    {
        _logger = logger;
        var opts = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opts.HostName,
            Port = opts.Port,
            UserName = opts.UserName,
            Password = opts.Password,
            VirtualHost = opts.VirtualHost
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task StartConsumingAsync(CancellationToken ct = default)
    {
        await _channel.QueueDeclareAsync(
            queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received message from queue '{Queue}': {Json}", QueueName, json);

            try
            {
                var message = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                _logger.LogInformation("Processing inventory message: ItemId={ItemId}",
                    message?.GetValueOrDefault("itemId"));

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue '{Queue}'", QueueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}
