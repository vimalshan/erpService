using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Infrastructure.Messaging.Options;

namespace ShipmentService.Infrastructure.Messaging.RabbitMQ.Consumers;

/// <summary>Background consumer that processes incoming shipment events from other services.</summary>
public sealed class ShipmentCreatedConsumer : BackgroundService
{
    private readonly ILogger<ShipmentCreatedConsumer> _logger;
    private readonly RabbitMQOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public ShipmentCreatedConsumer(
        IOptions<RabbitMQOptions> options,
        ILogger<ShipmentCreatedConsumer> logger,
        IServiceScopeFactory scopeFactory)
    {
        _options = options.Value;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _options.Host,
                    Port = _options.Port,
                    UserName = _options.UserName,
                    Password = _options.Password,
                    VirtualHost = _options.VirtualHost
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("shipment.exchange", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync("shipment.created.queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync("shipment.created.queue", "shipment.exchange", "shipment.created", cancellationToken: stoppingToken);

                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received shipment.created event: {Message}", message);

                    try
                    {
                        await ProcessShipmentCreatedAsync(message, stoppingToken);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing shipment.created message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync("shipment.created.queue", autoAck: false, consumer, stoppingToken);

                _logger.LogInformation("ShipmentCreatedConsumer connected and listening.");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("ShipmentCreatedConsumer shutting down.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ShipmentCreatedConsumer failed. Retrying in 30 seconds...");
                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
            finally
            {
                if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
                if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }
            }
        }
    }

    private async Task ProcessShipmentCreatedAsync(string message, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var payload = JsonSerializer.Deserialize<ShipmentCreatedMessage>(message);
        if (payload is null) return;
        _logger.LogInformation("Processing shipment created: {ShipmentNumber} for customer {CustomerId}",
            payload.ShipmentNumber, payload.CustomerId);
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
        if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }
        await base.StopAsync(cancellationToken);
    }

    private sealed record ShipmentCreatedMessage(string ShipmentNumber, int CustomerId, int WarehouseId, DateTime CreatedAt);
}
