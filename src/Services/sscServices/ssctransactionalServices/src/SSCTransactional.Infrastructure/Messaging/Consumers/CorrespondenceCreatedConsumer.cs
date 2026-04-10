using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SSCTransactional.Infrastructure.Settings;

namespace SSCTransactional.Infrastructure.Messaging.Consumers;

public class CorrespondenceCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CorrespondenceCreatedConsumer> _logger;
    private readonly RabbitMQSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;

    public CorrespondenceCreatedConsumer(IServiceScopeFactory scopeFactory, IOptions<RabbitMQSettings> options, ILogger<CorrespondenceCreatedConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = options.Value;
        _logger = logger;
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
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(_settings.TransactionExchange, ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync("correspondence.created", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync("correspondence.created", _settings.TransactionExchange, "correspondence.created", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("[Consumer] CorrespondenceCreated event received: {Body}", body);

                using var scope = _scopeFactory.CreateScope();
                // Handle correspondence created event — e.g. notify relevant users, update workflow status
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            };

            await _channel.BasicConsumeAsync("correspondence.created", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* shutting down */ }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Consumer] CorrespondenceCreatedConsumer could not connect to RabbitMQ broker. Consumer will not run.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
