using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DevelopmentService.Infrastructure.Messaging.Consumers;

public class BhrPlanApprovedConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BhrPlanApprovedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName  = "development.bhr-plan.created";
    private const string Exchange   = "development.events";
    private const string RoutingKey = "bhr-plan.created";

    public BhrPlanApprovedConsumer(
        IConfiguration configuration,
        ILogger<BhrPlanApprovedConsumer> logger)
    {
        _configuration = configuration;
        _logger        = logger;
    }

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
                _logger.LogWarning(ex,
                    "BhrPlanApprovedConsumer: RabbitMQ unavailable. Retrying in 30 seconds.");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var rabbitConfig = _configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory
        {
            HostName    = rabbitConfig["Host"] ?? "localhost",
            Port        = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672,
            UserName    = rabbitConfig["Username"] ?? "guest",
            Password    = rabbitConfig["Password"] ?? "guest",
            VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, Exchange, RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            _logger.LogInformation("Received BhrPlanCreated event: {Body}", body);
            // Add downstream processing here (e.g. HR notifications, approvals).
            await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        _logger.LogInformation("BhrPlanApprovedConsumer: Connected and consuming from {Queue}.", QueueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
