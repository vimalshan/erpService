using System.Text;
using System.Text.Json;
using EmployeeService.Shared.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmployeeService.Worker.Services;

public class RabbitMqEmployeeEventConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqEmployeeEventConsumer> _logger;

    public RabbitMqEmployeeEventConsumer(
        RabbitMqSettings settings,
        ILogger<RabbitMqEmployeeEventConsumer> logger)
    {
        _settings = settings;
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
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost
                };

                await using var connection = await factory.CreateConnectionAsync(stoppingToken);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await channel.QueueDeclareAsync(
                    queue: _settings.EmployeeEventsQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    passive: false,
                    noWait: false,
                    cancellationToken: stoppingToken);

                _logger.LogInformation("RabbitMQ consumer listening on queue {QueueName}", _settings.EmployeeEventsQueueName);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, eventArgs) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                        var message = JsonSerializer.Deserialize<EmployeeEventMessage>(json);

                        if (message is null)
                        {
                            _logger.LogWarning("Received empty or invalid employee event payload.");
                        }
                        else
                        {
                            _logger.LogInformation(
                                "Consumed employee event {EventType} for employee {EmployeeId}: {Description}",
                                message.EventType,
                                message.EmployeeId,
                                message.Description);
                        }

                        await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process employee event message.");
                    }
                };

                await channel.BasicConsumeAsync(
                    queue: _settings.EmployeeEventsQueueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed. Retrying in {RetryDelaySeconds} seconds.", _settings.ConsumerRetryDelaySeconds);
                await Task.Delay(TimeSpan.FromSeconds(_settings.ConsumerRetryDelaySeconds), stoppingToken);
            }
        }
    }
}