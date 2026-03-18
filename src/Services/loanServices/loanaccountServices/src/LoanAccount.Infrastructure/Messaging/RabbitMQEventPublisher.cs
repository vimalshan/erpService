using System.Text;
using System.Text.Json;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LoanAccount.Infrastructure.Messaging;

/// <summary>
/// Interface for publishing events to RabbitMQ
/// </summary>
public interface IEventPublisher
{
    Task PublishEventAsync<T>(T @event, string eventType, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// RabbitMQ event publisher implementation
/// </summary>
public class RabbitMQEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchangeName;
    private readonly ILogger<RabbitMQEventPublisher> _logger;

    public RabbitMQEventPublisher(
        IConnection connection,
        IConfiguration configuration,
        ILogger<RabbitMQEventPublisher> logger)
    {
        _connection = Guard.Against.Null(connection, nameof(connection));
        _logger = Guard.Against.Null(logger, nameof(logger));

        var rabbitMQSettings = configuration.GetSection("RabbitMQ");
        _exchangeName = rabbitMQSettings.GetValue<string>("ExchangeName") ?? "loan-account-exchange";

        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(
            exchange: _exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);
    }

    public async Task PublishEventAsync<T>(T @event, string eventType, CancellationToken cancellationToken = default) where T : class
    {
        Guard.Against.Null(@event, nameof(@event));
        Guard.Against.NullOrEmpty(eventType, nameof(eventType));

        try
        {
            var routingKey = $"loan.{eventType.ToLower()}";
            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Event published: {EventType} with routing key: {RoutingKey}", eventType, routingKey);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event: {EventType}", eventType);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null && _channel.IsOpen)
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection is not null && _connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }

        await Task.CompletedTask;
    }
}

/// <summary>
/// Interface for consuming events from RabbitMQ
/// </summary>
public interface IEventConsumer
{
    Task StartConsumingAsync(CancellationToken cancellationToken = default);
    void Stop();
}

/// <summary>
/// RabbitMQ event consumer implementation for loan events
/// </summary>
public class RabbitMQEventConsumer : IEventConsumer, IAsyncDisposable
{
    private readonly IConnection _connection;
    private IModel? _channel;
    private string? _queueName;
    private readonly ILogger<RabbitMQEventConsumer> _logger;
    private readonly IConfiguration _configuration;

    public RabbitMQEventConsumer(
        IConnection connection,
        IConfiguration configuration,
        ILogger<RabbitMQEventConsumer> logger)
    {
        _connection = Guard.Against.Null(connection, nameof(connection));
        _configuration = Guard.Against.Null(configuration, nameof(configuration));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);

            var rabbitMQSettings = _configuration.GetSection("RabbitMQ");
            var exchangeName = rabbitMQSettings.GetValue<string>("ExchangeName") ?? "loan-account-exchange";
            _queueName = rabbitMQSettings.GetValue<string>("QueueName") ?? "loan-account-events";

            // Declare queue
            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: _queueName,
                exchange: exchangeName,
                routingKey: "loan.*");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                await ProcessMessageAsync(ea);
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumerTag: "loan-consumer",
                consumer: consumer);

            _logger.LogInformation("RabbitMQ event consumer started for queue: {QueueName}", _queueName);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting event consumer");
            throw;
        }
    }

    private async Task ProcessMessageAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(
                "Event received - Routing Key: {RoutingKey}, Message: {Message}",
                ea.RoutingKey, message);

            // Process the message based on routing key
            switch (ea.RoutingKey)
            {
                case "loan.created":
                    await HandleLoanCreatedAsync(message);
                    break;
                case "loan.approved":
                    await HandleLoanApprovedAsync(message);
                    break;
                case "loan.disbursed":
                    await HandleLoanDisbursedAsync(message);
                    break;
                case "loan.paid":
                    await HandleEMIPaidAsync(message);
                    break;
                case "loan.settled":
                    await HandleLoanSettledAsync(message);
                    break;
                default:
                    _logger.LogWarning("Unknown event type: {RoutingKey}", ea.RoutingKey);
                    break;
            }

            _channel?.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            _channel?.BasicNack(ea.DeliveryTag, false, true); // Requeue message
        }

        await Task.CompletedTask;
    }

    private async Task HandleLoanCreatedAsync(string message)
    {
        _logger.LogInformation("Processing loan created event: {Message}", message);
        // Implement your business logic here
        await Task.CompletedTask;
    }

    private async Task HandleLoanApprovedAsync(string message)
    {
        _logger.LogInformation("Processing loan approved event: {Message}", message);
        // Implement your business logic here
        await Task.CompletedTask;
    }

    private async Task HandleLoanDisbursedAsync(string message)
    {
        _logger.LogInformation("Processing loan disbursed event: {Message}", message);
        // Implement your business logic here
        await Task.CompletedTask;
    }

    private async Task HandleEMIPaidAsync(string message)
    {
        _logger.LogInformation("Processing EMI paid event: {Message}", message);
        // Implement your business logic here
        await Task.CompletedTask;
    }

    private async Task HandleLoanSettledAsync(string message)
    {
        _logger.LogInformation("Processing loan settled event: {Message}", message);
        // Implement your business logic here
        await Task.CompletedTask;
    }

    public void Stop()
    {
        if (_channel is not null && _channel.IsOpen)
        {
            _channel.BasicCancel("loan-consumer");
            _logger.LogInformation("Event consumer stopped");
        }
    }

    public async ValueTask DisposeAsync()
    {
        Stop();
        if (_channel is not null)
        {
            _channel.Close();
            _channel.Dispose();
        }

        await Task.CompletedTask;
    }
}
