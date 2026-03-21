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
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
                logger.LogWarning(ex, "RabbitMQ connection failed. Retrying in 30 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password };
        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(exchange: "booking.events", type: ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        var queueDeclareResult = await channel.QueueDeclareAsync(queue: "booking.notifications", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(queue: queueDeclareResult.QueueName, exchange: "booking.events", routingKey: "booking.*", cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.LogInformation("Received booking event [{RoutingKey}]: {Message}", ea.RoutingKey, message);

            // Process the message based on routing key
            await ProcessMessage(ea.RoutingKey, message);

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(queue: queueDeclareResult.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        // Keep alive until cancellation
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task ProcessMessage(string routingKey, string message)
    {
        logger.LogInformation("Processing {RoutingKey} message", routingKey);
        // Add specific message processing logic here
        return Task.CompletedTask;
    }
}
