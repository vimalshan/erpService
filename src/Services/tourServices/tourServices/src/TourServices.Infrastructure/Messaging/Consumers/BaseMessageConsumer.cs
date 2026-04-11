using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace TourServices.Infrastructure.Messaging.Consumers;

public abstract class BaseMessageConsumer<TMessage> : BackgroundService
{
    protected readonly ILogger Logger;
    protected abstract string QueueName { get; }
    protected abstract string ExchangeName { get; }
    protected abstract string RoutingKey { get; }

    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;
    private const int MaxRetryDelaySeconds = 60;

    protected BaseMessageConsumer(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        Logger = logger;
    }

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
                Logger.LogWarning(ex, "{Consumer} connection failed. Retrying in {Delay}s...",
                    GetType().Name, retryDelay.TotalSeconds);
                await CleanupAsync();
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
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/",
            RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                var message = JsonSerializer.Deserialize<TMessage>(body);
                if (message is not null)
                    await HandleMessageAsync(message, stoppingToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing message from queue {Queue}", QueueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        Logger.LogInformation("Started consuming from {Queue}", QueueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task CleanupAsync()
    {
        try { if (_channel is not null) await _channel.CloseAsync(); } catch { /* best-effort */ }
        try { if (_connection is not null) await _connection.CloseAsync(); } catch { /* best-effort */ }
        _channel = null;
        _connection = null;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("{Consumer} stopping...", GetType().Name);
        await base.StopAsync(cancellationToken);
        await CleanupAsync();
    }

    protected abstract Task HandleMessageAsync(TMessage message, CancellationToken cancellationToken);

    public override void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _channel?.Dispose();
        base.Dispose();
    }
}
