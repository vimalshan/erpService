using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MeetingModule.Infrastructure.Messaging;

public class MeetingCreatedConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<MeetingCreatedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public MeetingCreatedConsumer(IOptions<RabbitMqSettings> settings, ILogger<MeetingCreatedConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(_settings.MeetingCreatedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(_settings.MeetingCreatedQueue, _settings.ExchangeName, "meeting.created", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received meeting.created: {Body}", body);

                    // Process the message (e.g., send notifications, update calendars)

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing meeting.created message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(_settings.MeetingCreatedQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            _logger.LogInformation("MeetingCreatedConsumer started listening on {Queue}", _settings.MeetingCreatedQueue);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* Shutdown */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MeetingCreatedConsumer encountered an error");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}

public class MeetingStatusChangedConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<MeetingStatusChangedConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public MeetingStatusChangedConsumer(IOptions<RabbitMqSettings> settings, ILogger<MeetingStatusChangedConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(_settings.MeetingStatusChangedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(_settings.MeetingStatusChangedQueue, _settings.ExchangeName, "meeting.status.*", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received meeting.status change: {Body}", body);

                    // Process the message (e.g., notify participants, update dashboards)

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing meeting.status message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(_settings.MeetingStatusChangedQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            _logger.LogInformation("MeetingStatusChangedConsumer started listening on {Queue}", _settings.MeetingStatusChangedQueue);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* Shutdown */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MeetingStatusChangedConsumer encountered an error");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
