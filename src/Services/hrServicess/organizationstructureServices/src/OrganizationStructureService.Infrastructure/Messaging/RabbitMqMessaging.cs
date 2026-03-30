using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace OrganizationStructureService.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "organization.events";
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
}

public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> options, ILogger<RabbitMqPublisher> logger)
    {
        _settings = options.Value;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);

        _logger.LogInformation("Published message to exchange {Exchange} with routing key {RoutingKey}",
            _settings.ExchangeName, routingKey);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}

public class BusinessCreatedConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<BusinessCreatedConsumer> _logger;
    private readonly string _queueName = "organization.business.created";

    public BusinessCreatedConsumer(IOptions<RabbitMqSettings> options, ILogger<BusinessCreatedConsumer> logger)
    {
        _logger = logger;
        var settings = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            VirtualHost = settings.VirtualHost
        };
        try
        {
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(settings.ExchangeName, ExchangeType.Topic, durable: true).GetAwaiter().GetResult();
            _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();
            _channel.QueueBindAsync(_queueName, settings.ExchangeName, "business.created").GetAwaiter().GetResult();
            _logger.LogInformation("BusinessCreatedConsumer connected to RabbitMQ. Exchange={Exchange} Queue={Queue}", settings.ExchangeName, _queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "BusinessCreatedConsumer failed to connect to RabbitMQ at {Host}:{Port}. Consumer will not be available.", settings.HostName, settings.Port);
            throw;
        }
    }

    public async Task StartConsumingAsync(CancellationToken ct)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received business.created event: {Message}", message);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing business.created message");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, ct);
            }
        };
        await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}

public class BusinessCreatedConsumerService : BackgroundService
{
    private readonly BusinessCreatedConsumer _consumer;

    public BusinessCreatedConsumerService(BusinessCreatedConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartConsumingAsync(stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
