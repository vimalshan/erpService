using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CashManagement.Infrastructure.Messaging.Settings;

namespace CashManagement.Infrastructure.Messaging.RabbitMQ;

public abstract class RabbitMqConsumerBase<TMessage> : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    protected readonly ILogger Logger;

    protected RabbitMqConsumerBase(IConnection connection, IChannel channel, ILogger logger)
    {
        _connection = connection;
        _channel = channel;
        Logger = logger;
    }

    public async Task StartConsumingAsync(string queueName, CancellationToken ct = default)
    {
        await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<TMessage>(json)!;
                await ProcessMessageAsync(message, ct);
                await _channel.BasicAckAsync(args.DeliveryTag, false, ct);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing RabbitMQ message from queue {Queue}", queueName);
                await _channel.BasicNackAsync(args.DeliveryTag, false, false, ct);
            }
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer, ct);
        Logger.LogInformation("Consumer started on queue: {Queue}", queueName);
    }

    protected abstract Task ProcessMessageAsync(TMessage message, CancellationToken ct);

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
