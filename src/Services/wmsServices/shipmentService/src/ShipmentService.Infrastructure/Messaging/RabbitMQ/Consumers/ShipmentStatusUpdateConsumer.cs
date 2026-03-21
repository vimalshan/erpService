using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;
using ShipmentService.Infrastructure.Messaging.Options;

namespace ShipmentService.Infrastructure.Messaging.RabbitMQ.Consumers;

/// <summary>Consumes status-update requests from other services (e.g. carrier webhooks via gateway).</summary>
public sealed class ShipmentStatusUpdateConsumer : BackgroundService
{
    private readonly ILogger<ShipmentStatusUpdateConsumer> _logger;
    private readonly RabbitMQOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public ShipmentStatusUpdateConsumer(
        IOptions<RabbitMQOptions> options,
        ILogger<ShipmentStatusUpdateConsumer> logger,
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
                await _channel.QueueDeclareAsync("shipment.status_update.queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync("shipment.status_update.queue", "shipment.exchange", "shipment.status_update_request", cancellationToken: stoppingToken);
                await _channel.BasicQosAsync(0, 10, false, stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var msg = JsonSerializer.Deserialize<StatusUpdateMessage>(body);
                        if (msg is not null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.Send(new UpdateShipmentStatusCommand(
                                msg.ShipmentId, msg.NewStatus, msg.Location, msg.Description, msg.UpdatedBy), stoppingToken);
                        }
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing status update message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync("shipment.status_update.queue", autoAck: false, consumer, stoppingToken);
                _logger.LogInformation("ShipmentStatusUpdateConsumer connected and listening.");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ShipmentStatusUpdateConsumer failed. Retrying in 30 seconds...");
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

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }

    private sealed record StatusUpdateMessage(int ShipmentId, string NewStatus, string? Location, string? Description, string? UpdatedBy);
}
