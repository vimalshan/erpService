using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookingService.Infrastructure.Messaging;

public class BookingMessageConsumer(
    string hostName,
    string userName,
    string password,
    ILogger<BookingMessageConsumer> logger) : BackgroundService
{
    private const int MaxRetryDelaySeconds = 60;
    private IConnection? _connection;
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retryDelay = TimeSpan.FromSeconds(5);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "RabbitMQ consumer connection failed. Retrying in {Delay}s...", retryDelay.TotalSeconds);
                await CleanupAsync();
                try { await Task.Delay(retryDelay, stoppingToken); }
                catch (OperationCanceledException) { break; }
                retryDelay = TimeSpan.FromSeconds(Math.Min(retryDelay.TotalSeconds * 2, MaxRetryDelaySeconds));
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password };
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(exchange: "booking.events", type: ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        var queueDeclareResult = await _channel.QueueDeclareAsync(queue: "booking.notifications", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue: queueDeclareResult.QueueName, exchange: "booking.events", routingKey: "booking.*", cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                logger.LogInformation("Received booking event [{RoutingKey}]: {Message}", ea.RoutingKey, message);

                await ProcessMessage(ea.RoutingKey, message);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message from booking.notifications");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: queueDeclareResult.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        logger.LogInformation("Started consuming from booking.notifications");

        // Keep alive until cancellation
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task CleanupAsync()
    {
        try { if (_channel is not null) await _channel.CloseAsync(); } catch { /* best-effort */ }
        try { if (_connection is not null) await _connection.CloseAsync(); } catch { /* best-effort */ }
        _channel = null;
        _connection = null;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("BookingMessageConsumer stopping...");
        await base.StopAsync(cancellationToken);
        await CleanupAsync();
    }

    private Task ProcessMessage(string routingKey, string message)
    {
        logger.LogInformation("Processing {RoutingKey} message", routingKey);
        // Add specific message processing logic here
        return Task.CompletedTask;
    }
}
