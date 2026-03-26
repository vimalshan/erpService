using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReferenceDataService.Infrastructure.Messaging.Consumers;

public class LovMasterCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<LovMasterCreatedConsumer> _logger;

    private const string ExchangeName = "reference-data";
    private const string QueueName = "lov-master-created";
    private const string RoutingKey = "lov.master.created";

    public LovMasterCreatedConsumer(
        IServiceScopeFactory scopeFactory,
        RabbitMqConnection connection,
        ILogger<LovMasterCreatedConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var channel = await _connection.GetChannelAsync();

                await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        _logger.LogInformation("Received LovMasterCreated message: {Message}", body);

                        using var scope = _scopeFactory.CreateScope();
                        // Process message here

                        await channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing LovMasterCreated message");
                        await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };

                await channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed for {Queue}. Retrying in 30 seconds...", QueueName);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
