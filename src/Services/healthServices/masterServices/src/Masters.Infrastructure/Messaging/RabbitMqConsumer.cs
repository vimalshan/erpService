using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Masters.Infrastructure.Messaging;

public abstract class RabbitMqConsumer<T> : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ILogger _logger;
    private readonly string _queueName;
    private readonly string _connectionString;

    protected RabbitMqConsumer(string connectionString, string queueName, ILogger logger)
    {
        _connectionString = connectionString;
        _logger = logger;
        _queueName = queueName;
    }

    private async Task<bool> TryInitializeAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory { Uri = new Uri(_connectionString) };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ for queue {Queue}, will retry", _queueName);
            return false;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry connection with backoff until the service stops
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_channel == null || !_channel.IsOpen)
            {
                if (!await TryInitializeAsync(stoppingToken))
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }
            }

            var consumer = new AsyncEventingBasicConsumer(_channel!);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageObject = JsonSerializer.Deserialize<T>(message);

                    if (messageObject != null)
                    {
                        await HandleMessageAsync(messageObject, stoppingToken);
                        await _channel!.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                        _logger.LogInformation("Successfully processed message from queue {Queue}", _queueName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {Queue}", _queueName);
                    await _channel!.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                }
            };

            try
            {
                await _channel!.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming from queue {Queue}, will reconnect", _queueName);
                _channel = null;
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                continue;
            }

            // Keep alive while channel is open
            while (!stoppingToken.IsCancellationRequested && _channel != null && _channel.IsOpen)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }

    protected abstract Task HandleMessageAsync(T message, CancellationToken cancellationToken);

    public override void Dispose()
    {
        if (_channel != null)
            _channel.CloseAsync().GetAwaiter().GetResult();
        if (_connection != null)
            _connection.CloseAsync().GetAwaiter().GetResult();
        base.Dispose();
    }
}
