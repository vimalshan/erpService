using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Masters.Infrastructure.Messaging;

public abstract class RabbitMqConsumer<T> : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger _logger;
    private readonly string _queueName;

    protected RabbitMqConsumer(string connectionString, string queueName, ILogger logger)
    {
        _logger = logger;
        _queueName = queueName;

        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null).GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
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
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                    _logger.LogInformation("Successfully processed message from queue {Queue}", _queueName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {Queue}", _queueName);
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        
        await Task.CompletedTask;
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
