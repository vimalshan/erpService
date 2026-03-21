using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UnitService.Infrastructure.Messaging.Consumers;

public class EquipmentStatusChangedConsumer : BackgroundService
{
    private readonly ILogger<EquipmentStatusChangedConsumer> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private IConnection? _connection;
    private IChannel? _channel;

    public EquipmentStatusChangedConsumer(ILogger<EquipmentStatusChangedConsumer> logger,
        string hostName, string userName, string password)
    {
        _logger = logger;
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("unit-events", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                var queueDeclare = await _channel.QueueDeclareAsync("equipment-status-changed-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queueDeclare.QueueName, "unit-events", "equipment.status.changed", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Received equipment status changed event: {Message}", message);

                    // Process the message here
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _channel.BasicConsumeAsync(queueDeclare.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed for EquipmentStatusChangedConsumer. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
