using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EmployeeManagement.Infrastructure.Messaging;

/// <summary>Background consumer listening to HR domain events from RabbitMQ.</summary>
public sealed class EmployeeEventConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmployeeEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public EmployeeEventConsumer(IServiceScopeFactory scopeFactory, ILogger<EmployeeEventConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry loop — keeps trying to connect if RabbitMQ is not yet available
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                const string exchange = "hr.events";
                const string queue = "hr.employee.events";
                await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync(queue, true, false, false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queue, exchange, "employee.created", cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queue, exchange, "employee.promoted", cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queue, exchange, "employee.transferred", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received RabbitMQ message on queue hr.employee.events: {Body}", body);

                    try
                    {
                        var routingKey = ea.RoutingKey;
                        using var scope = _scopeFactory.CreateScope();
                        _logger.LogInformation("Processed event: {RoutingKey}", routingKey);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process RabbitMQ message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };

                await _channel.BasicConsumeAsync("hr.employee.events", false, consumer, stoppingToken);
                _logger.LogInformation("RabbitMQ consumer connected and listening on hr.employee.events");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break; // Graceful shutdown
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ consumer connection failed. Retrying in 30 seconds...");

                if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
                if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }

                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
