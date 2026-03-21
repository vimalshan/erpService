using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinanceService.Infrastructure.Messaging.Consumers;

public class PaymentProcessedConsumer : BackgroundService
{
    private readonly RabbitMqConnection _rabbitMqConnection;
    private readonly ILogger<PaymentProcessedConsumer> _logger;
    private IChannel? _channel;

    public PaymentProcessedConsumer(RabbitMqConnection rabbitMqConnection, ILogger<PaymentProcessedConsumer> logger)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var connection = await _rabbitMqConnection.GetConnectionAsync(stoppingToken);
                _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("finance", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                var queueDeclare = await _channel.QueueDeclareAsync("finance.payment.processed", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queueDeclare.QueueName, "finance", "payment.processed", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Payment processed event received: {Message}", body);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                };

                await _channel.BasicConsumeAsync(queueDeclare.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed for PaymentProcessedConsumer. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is { IsOpen: true })
            await _channel.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
