using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using BookingService.Infrastructure.Messaging;

namespace BookingService.Infrastructure.Messaging;

/// <summary>
/// Background consumer for booking-related RabbitMQ messages.
/// Subscribes to booking.created, booking.confirmed, booking.cancelled routing keys.
/// </summary>
public class BookingMessageConsumer : BackgroundService
{
    private readonly ILogger<BookingMessageConsumer> _logger;
    private readonly RabbitMqOptions _options;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "booking.service.queue";

    public BookingMessageConsumer(IOptions<RabbitMqOptions> options, ILogger<BookingMessageConsumer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            await _channel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            foreach (var routingKey in new[] { "booking.created", "booking.confirmed", "booking.cancelled" })
                await _channel.QueueBindAsync(QueueName, _options.ExchangeName, routingKey, cancellationToken: stoppingToken);

            await _channel.BasicQosAsync(0, 10, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received [{RoutingKey}]: {Message}", ea.RoutingKey, message);

                await ProcessMessageAsync(ea.RoutingKey, message, stoppingToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            _logger.LogInformation("BookingMessageConsumer started, listening on queue: {Queue}", QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* graceful stop */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BookingMessageConsumer faulted");
        }
    }

    private Task ProcessMessageAsync(string routingKey, string message, CancellationToken ct)
    {
        _logger.LogInformation("Processing {RoutingKey}: {Message}", routingKey, message);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
