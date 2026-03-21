using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace TravelService.Infrastructure.Messaging;

/// <summary>Background consumer for tour plan events from RabbitMQ.</summary>
public class TourPlanEventConsumer : BackgroundService
{
    private readonly ILogger<TourPlanEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public TourPlanEventConsumer(ILogger<TourPlanEventConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync("travel.events", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            var queueResult = await _channel.QueueDeclareAsync("travel.tourplan.events", durable: true, exclusive: false,
                autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(queueResult.QueueName, "travel.events", "tourplan.*", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("Received event on {RoutingKey}: {Body}", ea.RoutingKey, body);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };
            await _channel.BasicConsumeAsync(queueResult.QueueName, false, consumer, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TourPlanEventConsumer error");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
