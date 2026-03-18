using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using LoanApplication.Domain.Interfaces;

namespace LoanApplication.Infrastructure.Messaging;

/// <summary>
/// RabbitMQ message bus implementation (simplified for .NET 10)
/// </summary>
public class RabbitMQMessageBus : IMessageBus
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQMessageBus> _logger;
    private bool _isInitialized = false;

    public RabbitMQMessageBus(RabbitMQSettings settings, ILogger<RabbitMQMessageBus> logger)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logger.LogInformation("RabbitMQ message bus created with host {HostName}:{Port}", settings.HostName, settings.Port);
    }

    private async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_isInitialized)
            return;

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

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            _isInitialized = true;

            _logger.LogInformation("RabbitMQ message bus initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize RabbitMQ message bus");
            throw;
        }
    }

    private const string DefaultExchange = "loan.application.events";

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken cancellationToken = default) where T : class
    {
        await InitializeAsync(cancellationToken);
        
        if (_channel == null)
            throw new InvalidOperationException("Channel is not initialized");

        try
        {
            // Declare exchange
            await _channel.ExchangeDeclareAsync(DefaultExchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);

            // Serialize message
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Create basic properties
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            // Publish message
            await _channel.BasicPublishAsync(DefaultExchange, routingKey, mandatory: false, properties, new ReadOnlyMemory<byte>(body), cancellationToken);

            _logger.LogInformation("Message published to {Exchange}/{RoutingKey}", DefaultExchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange}/{RoutingKey}", DefaultExchange, routingKey);
            throw;
        }
    }

    public async Task SubscribeAsync<T>(string exchange, string queue, string routingKey, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class
    {
        await InitializeAsync(cancellationToken);
        
        if (_channel == null)
            throw new InvalidOperationException("Channel is not initialized");

        try
        {
            // Declare exchange and queue
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(queue, exchange, routingKey, cancellationToken: cancellationToken);

            _logger.LogInformation("Subscribed to {Exchange}/{Queue} with routing key {RoutingKey}", exchange, queue, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to subscribe to {Exchange}/{Queue}", exchange, queue);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
            
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
            
            _logger.LogInformation("RabbitMQ message bus closed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing RabbitMQ connection");
        }
    }
}

