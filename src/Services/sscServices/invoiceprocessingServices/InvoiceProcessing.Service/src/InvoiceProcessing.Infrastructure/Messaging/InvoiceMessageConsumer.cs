using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InvoiceProcessing.Infrastructure.Messaging;

public class InvoiceMessageConsumer(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<InvoiceMessageConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672")
            };

            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.ExchangeDeclareAsync("invoice-processing", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            var queueDeclareResult = await channel.QueueDeclareAsync("invoice-processing-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueDeclareResult.QueueName, "invoice-processing", "document.*", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    logger.LogInformation("Received message: {RoutingKey} - {Message}", ea.RoutingKey, message);

                    using var scope = scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    // Process based on routing key
                    switch (ea.RoutingKey)
                    {
                        case "document.created":
                            logger.LogInformation("Processing document created event");
                            break;
                        case "document.approved":
                            logger.LogInformation("Processing document approved event");
                            break;
                        case "document.cancelled":
                            logger.LogInformation("Processing document cancelled event");
                            break;
                        default:
                            logger.LogInformation("Unhandled routing key: {RoutingKey}", ea.RoutingKey);
                            break;
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing message");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await channel.BasicConsumeAsync(queueDeclareResult.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "RabbitMQ consumer failed to start. Service will continue without message consumption.");
        }
    }
}
