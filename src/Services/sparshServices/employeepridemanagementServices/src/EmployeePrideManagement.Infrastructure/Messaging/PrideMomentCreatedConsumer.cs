using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmployeePrideManagement.Infrastructure.Messaging;

public class PrideMomentCreatedConsumer : BackgroundService
{
    private readonly ILogger<PrideMomentCreatedConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "pride-moment-created";

    public PrideMomentCreatedConsumer(IConfiguration configuration, ILogger<PrideMomentCreatedConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(
                    queue: QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Received pride-moment-created message: {Message}", message);

                    // Process the message (e.g., send notifications, update analytics)
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                };

                await _channel.BasicConsumeAsync(QueueName, false, consumer, stoppingToken);

                _logger.LogInformation("PrideMomentCreatedConsumer started listening on queue: {Queue}", QueueName);

                // Keep running until cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("PrideMomentCreatedConsumer stopping.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PrideMomentCreatedConsumer could not connect to RabbitMQ. Retrying in 30 seconds...");

                if (_channel is { IsOpen: true })
                    await _channel.CloseAsync(stoppingToken);
                if (_connection is { IsOpen: true })
                    await _connection.CloseAsync(stoppingToken);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is { IsOpen: true })
            await _channel.CloseAsync(cancellationToken);
        if (_connection is { IsOpen: true })
            await _connection.CloseAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}
