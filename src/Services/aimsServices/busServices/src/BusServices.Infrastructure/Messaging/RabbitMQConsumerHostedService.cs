using BusServices.Infrastructure.Messaging.RabbitMQ;
using BusServices.Infrastructure.Messaging.RabbitMQ.Consumers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace BusServices.Infrastructure.Messaging;

/// <summary>
/// Background hosted service that starts all RabbitMQ consumers.
/// </summary>
public sealed class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQConsumerHostedService> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public RabbitMQConsumerHostedService(
        IOptions<RabbitMQSettings> options,
        ILogger<RabbitMQConsumerHostedService> logger,
        ILoggerFactory loggerFactory)
    {
        _settings = options.Value;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            var busRegisteredConsumer = new BusRegisteredConsumer(
                _settings.BusRegisteredQueue,
                _loggerFactory.CreateLogger<BusRegisteredConsumer>());
            var employeeAssignedConsumer = new EmployeeAssignedConsumer(
                _settings.EmployeeAssignedQueue,
                _loggerFactory.CreateLogger<EmployeeAssignedConsumer>());

            await Task.WhenAll(
                busRegisteredConsumer.StartAsync(factory, stoppingToken),
                employeeAssignedConsumer.StartAsync(factory, stoppingToken));

            _logger.LogInformation("RabbitMQ consumers started.");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("RabbitMQ consumers stopping.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ consumer service failed to start. Continuing without consumers.");
        }
    }
}
