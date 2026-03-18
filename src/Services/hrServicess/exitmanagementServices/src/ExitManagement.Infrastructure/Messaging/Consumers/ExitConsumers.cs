using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ExitManagement.Infrastructure.Messaging.Consumers;

/// <summary>
/// Background consumer for the "exit-initiated" queue.
/// </summary>
public class ExitInitiatedConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExitInitiatedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public ExitInitiatedConsumer(IConfiguration configuration, ILogger<ExitInitiatedConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
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

                await _channel.QueueDeclareAsync("exit-initiated", durable: true, exclusive: false,
                    autoDelete: false, arguments: null, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("[Consumer] exit-initiated received: {Body}", body);

                    // Process the initiated exit (e.g., send notification, trigger workflow)
                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                };

                await _channel.BasicConsumeAsync("exit-initiated", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                _logger.LogInformation("ExitInitiatedConsumer connected to RabbitMQ and listening.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ExitInitiatedConsumer: RabbitMQ unavailable. Retrying in 30 seconds...");
                await CleanupAsync();
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken).ConfigureAwait(false);
            }
        }
    }

    private async Task CleanupAsync()
    {
        try { if (_channel is not null) await _channel.DisposeAsync(); } catch { /* ignore */ }
        try { if (_connection is not null) await _connection.DisposeAsync(); } catch { /* ignore */ }
        _channel = null;
        _connection = null;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await CleanupAsync();
        await base.StopAsync(cancellationToken);
    }
}

/// <summary>
/// Background consumer for the "exit-revoked" queue.
/// </summary>
public class ExitRevokedConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExitRevokedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public ExitRevokedConsumer(IConfiguration configuration, ILogger<ExitRevokedConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
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

                await _channel.QueueDeclareAsync("exit-revoked", durable: true, exclusive: false,
                    autoDelete: false, arguments: null, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("[Consumer] exit-revoked received: {Body}", body);

                    // Process the revoked exit (e.g., restore access, notify payroll)
                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                };

                await _channel.BasicConsumeAsync("exit-revoked", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                _logger.LogInformation("ExitRevokedConsumer connected to RabbitMQ and listening.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ExitRevokedConsumer: RabbitMQ unavailable. Retrying in 30 seconds...");
                await CleanupAsync();
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken).ConfigureAwait(false);
            }
        }
    }

    private async Task CleanupAsync()
    {
        try { if (_channel is not null) await _channel.DisposeAsync(); } catch { /* ignore */ }
        try { if (_connection is not null) await _connection.DisposeAsync(); } catch { /* ignore */ }
        _channel = null;
        _connection = null;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await CleanupAsync();
        await base.StopAsync(cancellationToken);
    }
}
