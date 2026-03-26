using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CanteenTransactionService.Infrastructure.Messaging.RabbitMQ;

namespace CanteenTransactionService.Infrastructure.Messaging.Consumers;

public class CanteenTransactionConsumer : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<CanteenTransactionConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string QueueName = "canteen.transaction.queue";
    private const string RoutingKey = "canteen.transaction.#";

    public CanteenTransactionConsumer(IOptions<RabbitMQSettings> settings, ILogger<CanteenTransactionConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.QueueBindAsync(
                queue: QueueName,
                exchange: _settings.ExchangeName,
                routingKey: RoutingKey,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received message: {Message}", body);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            _logger.LogInformation("CanteenTransactionConsumer started consuming from {Queue}", QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CanteenTransactionConsumer encountered an error");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
