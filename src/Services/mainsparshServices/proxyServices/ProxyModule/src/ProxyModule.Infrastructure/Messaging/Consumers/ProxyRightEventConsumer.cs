using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProxyModule.Infrastructure.Messaging.Consumers;

public class ProxyRightEventConsumer : BackgroundService
{
    private readonly ILogger<ProxyRightEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string ExchangeName = "proxy-module-exchange";
    private const string QueueName = "proxy-right-events";
    private const string RoutingKey = "proxy.right.*";

    public ProxyRightEventConsumer(IConfiguration configuration, ILogger<ProxyRightEventConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry loop — keeps the BackgroundService alive even when RabbitMQ is unavailable.
        var retryDelays = new[] { 5, 10, 20, 30, 60 };
        int attempt = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break; // Normal shutdown
            }
            catch (Exception ex)
            {
                int delaySeconds = attempt < retryDelays.Length ? retryDelays[attempt] : 60;
                attempt++;
                _logger.LogWarning(ex,
                    "RabbitMQ consumer disconnected. Retrying in {Delay}s (attempt {Attempt}).",
                    delaySeconds, attempt);

                try { await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ consumer connected to {Host}.", factory.HostName);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("Received message: {RoutingKey} - {Body}", ea.RoutingKey, body);

                if (ea.RoutingKey.EndsWith(".created"))
                    await HandleProxyRightCreated(body);
                else if (ea.RoutingKey.EndsWith(".deactivated"))
                    await HandleProxyRightDeactivated(body);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer, stoppingToken);

        // Keep alive until cancellation or connection drop
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task HandleProxyRightCreated(string body)
    {
        _logger.LogInformation("Processing ProxyRightCreated event: {Body}", body);
        return Task.CompletedTask;
    }

    private Task HandleProxyRightDeactivated(string body)
    {
        _logger.LogInformation("Processing ProxyRightDeactivated event: {Body}", body);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel?.IsOpen == true) await _channel.CloseAsync(cancellationToken);
        if (_connection?.IsOpen == true) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
