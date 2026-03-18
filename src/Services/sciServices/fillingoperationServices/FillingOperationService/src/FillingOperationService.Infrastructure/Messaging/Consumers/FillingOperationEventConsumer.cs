using FillingOperationService.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FillingOperationService.Infrastructure.Messaging.Consumers;

public class FillingOperationEventConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<FillingOperationEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public FillingOperationEventConsumer(IConfiguration configuration, ILogger<FillingOperationEventConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync("filling-operations-events", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received filling operation event: {Body}", body);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                };

                await _channel.BasicConsumeAsync("filling-operations-events", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                _logger.LogInformation("RabbitMQ consumer connected to filling-operations-events queue.");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed. Retrying in 30 seconds...");

                if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
                if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
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
