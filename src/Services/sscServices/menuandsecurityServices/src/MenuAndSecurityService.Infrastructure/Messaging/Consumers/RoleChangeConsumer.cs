using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MenuAndSecurityService.Infrastructure.Messaging.Consumers;

public class RoleChangeConsumer : BackgroundService
{
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<RoleChangeConsumer> _logger;

    public RoleChangeConsumer(RabbitMqConnection connection, ILogger<RoleChangeConsumer> logger)
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
            var queueResult = await channel.QueueDeclareAsync("role-change-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueResult.QueueName, "menu-exchange", "menu.created", cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueResult.QueueName, "menu-exchange", "menu.updated", cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueResult.QueueName, "menu-exchange", "menu.deleted", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("RoleChangeConsumer received: {RoutingKey} - {Message}",
                        ea.RoutingKey, body);

                    await ProcessRoleChangeEvent(ea.RoutingKey, body);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing role change message");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await channel.BasicConsumeAsync(queueResult.QueueName, false, consumer, stoppingToken);

            _logger.LogInformation("RoleChangeConsumer started listening");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("RoleChangeConsumer stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RoleChangeConsumer error");
        }
    }

    private Task ProcessRoleChangeEvent(string routingKey, string messageBody)
    {
        using var doc = JsonDocument.Parse(messageBody);
        _logger.LogInformation("Processing role change event [{RoutingKey}]: {Event}",
            routingKey, doc.RootElement.ToString());
        return Task.CompletedTask;
    }
}
