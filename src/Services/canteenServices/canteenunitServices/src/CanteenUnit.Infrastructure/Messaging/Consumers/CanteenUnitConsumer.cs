using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CanteenUnit.Infrastructure.Messaging.Consumers;

public class CanteenUnitConsumer : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<CanteenUnitConsumer> _logger;
    private readonly IConfiguration _config;
    private IConnection? _connection;
    private IChannel? _channel;

    public CanteenUnitConsumer(IServiceProvider services, ILogger<CanteenUnitConsumer> logger, IConfiguration config)
    {
        _services = services;
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"] ?? "localhost",
                UserName = _config["RabbitMQ:Username"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest"
            };
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            const string queueName = "canteen-unit-events";
            await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("CanteenUnit event received: {Body}", body);

                try
                {
                    using var scope = _services.CreateScope();
                    await ProcessMessageAsync(body, scope.ServiceProvider, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing CanteenUnit event");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer,
                cancellationToken: stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Host is shutting down — suppress gracefully
        }
        catch (Exception ex)
        {
            try { _logger.LogWarning(ex, "RabbitMQ consumer could not start — service will run without it"); }
            catch { /* logger may be disposed during shutdown */ }
        }
    }

    private static Task ProcessMessageAsync(string body, IServiceProvider services, CancellationToken ct)
    {
        // Extend here: deserialize event type and dispatch with MediatR
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
