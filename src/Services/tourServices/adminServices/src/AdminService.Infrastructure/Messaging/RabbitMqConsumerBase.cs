using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AdminService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<T> : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    protected abstract string QueueName { get; }
    protected abstract string ExchangeName { get; }
    protected abstract string RoutingKey { get; }

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private const int MaxRetryDelaySeconds = 60;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retryDelay = TimeSpan.FromSeconds(5);

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
                _logger.LogWarning(ex, "RabbitMQ consumer for {Queue} failed to connect. Retrying in {Delay}s...", QueueName, retryDelay.TotalSeconds);
                await CleanupChannelAsync();
                try { await Task.Delay(retryDelay, stoppingToken); }
                catch (OperationCanceledException) { break; }
                retryDelay = TimeSpan.FromSeconds(Math.Min(retryDelay.TotalSeconds * 2, MaxRetryDelaySeconds));
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

        await _channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKey, cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(body);
                if (message is not null)
                    await HandleMessageAsync(message, stoppingToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from {Queue}", QueueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        _logger.LogInformation("Started consuming from {Queue}", QueueName);

        // Keep the background service running until cancelled
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task CleanupChannelAsync()
    {
        try { if (_channel is not null) await _channel.CloseAsync(); } catch { /* best-effort */ }
        try { if (_connection is not null) await _connection.CloseAsync(); } catch { /* best-effort */ }
        _channel = null;
        _connection = null;
    }

    protected abstract Task HandleMessageAsync(T message, CancellationToken ct);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await CleanupChannelAsync();
        await base.StopAsync(cancellationToken);
    }
}
