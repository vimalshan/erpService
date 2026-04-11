using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TourPlanService.Domain.Events;

namespace TourPlanService.Infrastructure.Messaging.RabbitMq;

public sealed class TourPlanCreatedConsumer(
    IOptions<RabbitMqOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<TourPlanCreatedConsumer> logger) : BackgroundService
{
    private const int MaxRetryDelaySeconds = 60;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "tourplan.created";

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
                logger.LogWarning(ex, "TourPlanCreated consumer connection failed. Retrying in {Delay}s...", retryDelay.TotalSeconds);
                await CleanupAsync();
                try { await Task.Delay(retryDelay, stoppingToken); }
                catch (OperationCanceledException) { break; }
                retryDelay = TimeSpan.FromSeconds(Math.Min(retryDelay.TotalSeconds * 2, MaxRetryDelaySeconds));
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var opt = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opt.HostName,
            Port = opt.Port,
            UserName = opt.UserName,
            Password = opt.Password,
            VirtualHost = opt.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, opt.ExchangeName, "tourplan.created", cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(0, 1, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            logger.LogInformation("Received TourPlanCreated event: {Body}", body);

            try
            {
                // Process the event - e.g., send notifications
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing TourPlanCreated event");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);
        logger.LogInformation("Started consuming from {Queue}", QueueName);

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
        logger.LogInformation("TourPlanCreatedConsumer stopping...");
        await base.StopAsync(cancellationToken);
        await CleanupAsync();
    }
}

public sealed class TourPlanApprovedConsumer(
    IOptions<RabbitMqOptions> options,
    ILogger<TourPlanApprovedConsumer> logger) : BackgroundService
{
    private const int MaxRetryDelaySeconds = 60;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "tourplan.approved";

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
                logger.LogWarning(ex, "TourPlanApproved consumer connection failed. Retrying in {Delay}s...", retryDelay.TotalSeconds);
                await CleanupAsync();
                try { await Task.Delay(retryDelay, stoppingToken); }
                catch (OperationCanceledException) { break; }
                retryDelay = TimeSpan.FromSeconds(Math.Min(retryDelay.TotalSeconds * 2, MaxRetryDelaySeconds));
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var opt = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opt.HostName,
            Port = opt.Port,
            UserName = opt.UserName,
            Password = opt.Password,
            VirtualHost = opt.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, opt.ExchangeName, "tourplan.approved", cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            logger.LogInformation("Received TourPlanApproved event: {Body}", body);
            await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);
        logger.LogInformation("Started consuming from {Queue}", QueueName);

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
        logger.LogInformation("TourPlanApprovedConsumer stopping...");
        await base.StopAsync(cancellationToken);
        await CleanupAsync();
    }
}
