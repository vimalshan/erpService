using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MenuAndSecurityService.Infrastructure.Messaging.Consumers;

public class MenuAccessConsumer : BackgroundService
{
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<MenuAccessConsumer> _logger;

    public MenuAccessConsumer(RabbitMqConnection connection, ILogger<MenuAccessConsumer> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var channel = await _connection.GetChannelAsync();
            await channel.ExchangeDeclareAsync("menu-exchange", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            var queueResult = await channel.QueueDeclareAsync("menu-access-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueResult.QueueName, "menu-exchange", "menu.access.*", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("MenuAccessConsumer received: {Message}", body);

                    // Process the menu access event
                    await ProcessMenuAccessEvent(body);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing menu access message");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await channel.BasicConsumeAsync(queueResult.QueueName, false, consumer, stoppingToken);

            _logger.LogInformation("MenuAccessConsumer started listening");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("MenuAccessConsumer stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MenuAccessConsumer error");
        }
    }

    private Task ProcessMenuAccessEvent(string messageBody)
    {
        using var doc = JsonDocument.Parse(messageBody);
        _logger.LogInformation("Processing menu access event: {Event}", doc.RootElement.ToString());
        return Task.CompletedTask;
    }
}
