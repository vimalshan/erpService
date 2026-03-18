using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace RiskService.Infrastructure.Messaging;

public class RiskEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RiskEventConsumer> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private IConnection? _connection;
    private IChannel? _channel;

    public RiskEventConsumer(IServiceProvider serviceProvider, ILogger<RiskEventConsumer> logger,
        string hostName = "localhost", string userName = "guest", string password = "guest")
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync("risk.events", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            var queueDeclare = await _channel.QueueDeclareAsync("risk.service.queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(queueDeclare.QueueName, "risk.events", "risk.#", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message on {RoutingKey}: {Message}", ea.RoutingKey, message);

                    // Process message based on routing key
                    await ProcessMessage(ea.RoutingKey, message);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await _channel.BasicConsumeAsync(queueDeclare.QueueName, false, consumer, stoppingToken);

            _logger.LogInformation("RiskEventConsumer started. Listening for messages...");

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("RiskEventConsumer stopping...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RiskEventConsumer encountered an error");
        }
    }

    private Task ProcessMessage(string routingKey, string message)
    {
        _logger.LogInformation("Processing {RoutingKey}: {Message}", routingKey, message);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) { await _channel.CloseAsync(cancellationToken); _channel.Dispose(); }
        if (_connection is not null) { await _connection.CloseAsync(cancellationToken); _connection.Dispose(); }
        await base.StopAsync(cancellationToken);
    }
}
