using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserManagement.Infrastructure.Messaging.Options;

namespace UserManagement.Infrastructure.Messaging.Consumers;

/// <summary>Listens for UserPolicy events on RabbitMQ and processes them.</summary>
public class UserPolicyEventConsumer(
    IOptions<RabbitMqOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<UserPolicyEventConsumer> logger) : BackgroundService
{
    private readonly RabbitMqOptions _options = options.Value;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "user_policy_events";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry loop — keeps trying to connect to RabbitMQ without crashing the host
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _options.Host,
                    Port     = _options.Port,
                    UserName = _options.Username,
                    Password = _options.Password,
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel    = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(
                    queue: QueueName, durable: true, exclusive: false,
                    autoDelete: false, arguments: null, cancellationToken: stoppingToken);

                logger.LogInformation("UserPolicyEventConsumer connected to RabbitMQ. Listening on queue '{Queue}'.", QueueName);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.Span);
                        logger.LogInformation("Received user policy event: {Body}", body);
                        await ProcessEventAsync(body, stoppingToken);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing user policy event");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("UserPolicyEventConsumer stopping.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "UserPolicyEventConsumer could not connect to RabbitMQ. Retrying in 30 seconds...");
                await CleanupConnectionAsync();
                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }

        await CleanupConnectionAsync();
    }

    private async Task CleanupConnectionAsync()
    {
        try
        {
            if (_channel is not null) { await _channel.CloseAsync(); _channel = null; }
            if (_connection is not null) { await _connection.CloseAsync(); _connection = null; }
        }
        catch { /* ignore cleanup errors */ }
    }

    private async Task ProcessEventAsync(string body, CancellationToken cancellationToken)
    {
        // Extend here to dispatch to MediatR handlers via IServiceScope
        using var scope = scopeFactory.CreateScope();
        // var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        await CleanupConnectionAsync();
    }
}
