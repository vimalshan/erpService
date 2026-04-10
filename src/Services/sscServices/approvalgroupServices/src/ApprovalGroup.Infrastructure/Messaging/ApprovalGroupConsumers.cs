using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApprovalGroup.Infrastructure.Messaging;

public class ApprovalGroupCreatedConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ApprovalGroupCreatedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "approval_group.created";

    public ApprovalGroupCreatedConsumer(IOptions<RabbitMqSettings> settings,
        IServiceScopeFactory scopeFactory, ILogger<ApprovalGroupCreatedConsumer> logger)
    {
        _settings = settings.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        try
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(QueueName, _settings.ExchangeName, "approval_group.created", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("Received approval_group.created event: {Body}", body);

                // Process message using scoped services
                using var scope = _scopeFactory.CreateScope();
                // Add scoped processing logic here

                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Consumer] {ConsumerName} could not connect to RabbitMQ broker. Consumer will not run.",
                nameof(ApprovalGroupCreatedConsumer));
        }
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.CloseAsync(ct);
        if (_connection is not null) await _connection.CloseAsync(ct);
        await base.StopAsync(ct);
    }
}
