using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ItemMasterService.Application.DTOs;
using ItemMasterService.Infrastructure.Messaging.RabbitMQ;

namespace ItemMasterService.Infrastructure.Messaging.Consumers;

/// <summary>Background consumer for canteen item created events from other services.</summary>
public class CanteenItemCreatedConsumer : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<CanteenItemCreatedConsumer> _logger;
    private readonly RabbitMQSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "itemmaster.created.queue";

    public CanteenItemCreatedConsumer(
        IServiceProvider services,
        IOptions<RabbitMQSettings> settings,
        ILogger<CanteenItemCreatedConsumer> logger)
    {
        _services = services;
        _settings = settings.Value;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, "topic", durable: true, autoDelete: false, cancellationToken: ct);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
            await _channel.QueueBindAsync(QueueName, _settings.ExchangeName, "canteen.item.created", cancellationToken: ct);
            _logger.LogInformation("[Consumer] CanteenItemCreatedConsumer connected and listening.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Consumer] CanteenItemCreatedConsumer could not connect to RabbitMQ. Consumer will be inactive.");
            if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
            if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }
        }

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null) return;

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = JsonSerializer.Deserialize<CanteenItemMasterDto>(body);
                if (dto is not null)
                {
                    _logger.LogInformation("[Consumer] Received CanteenItemCreated: ItemCode={ItemCode}", dto.ItemCode);
                    // Process the consumed event (e.g., notify other aggregates)
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Consumer] Error processing CanteenItemCreated message.");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(ct);
    }
}

/// <summary>Background consumer for price update events.</summary>
public class CanteenItemPriceUpdatedConsumer : BackgroundService
{
    private readonly ILogger<CanteenItemPriceUpdatedConsumer> _logger;
    private readonly RabbitMQSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;
    private const string QueueName = "itemmaster.price.updated.queue";

    public CanteenItemPriceUpdatedConsumer(
        IOptions<RabbitMQSettings> settings,
        ILogger<CanteenItemPriceUpdatedConsumer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken ct)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, "topic", durable: true, autoDelete: false, cancellationToken: ct);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
            await _channel.QueueBindAsync(QueueName, _settings.ExchangeName, "canteen.item.price.updated", cancellationToken: ct);
            _logger.LogInformation("[Consumer] CanteenItemPriceUpdatedConsumer connected and listening.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[Consumer] CanteenItemPriceUpdatedConsumer could not connect to RabbitMQ. Consumer will be inactive.");
            if (_channel is not null) { await _channel.DisposeAsync(); _channel = null; }
            if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }
        }

        await base.StartAsync(ct);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null) return;

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = JsonSerializer.Deserialize<CanteenItemPriceMasterDto>(body);
                if (dto is not null)
                    _logger.LogInformation("[Consumer] Received PriceUpdated: ItemCode={ItemCode}", dto.ItemCode);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Consumer] Error processing PriceUpdated message.");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(ct);
    }
}
