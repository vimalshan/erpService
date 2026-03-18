using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailNotification.Infrastructure.Messaging;

/// <summary>
/// Consumer for email type created events
/// </summary>
public class EmailTypeCreatedConsumer : IMessageConsumer, IAsyncDisposable
{
    private readonly IRabbitMqConnection _connection;
    private readonly ILogger<EmailTypeCreatedConsumer> _logger;
    private IModel? _channel;
    private AsyncEventingBasicConsumer? _consumer;

    public EmailTypeCreatedConsumer(IRabbitMqConnection connection, ILogger<EmailTypeCreatedConsumer> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Starts consuming email type created events
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _connection.Connect();
            }

            _channel = _connection.Connection.CreateModel();

            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: EmailNotificationQueues.EmailNotificationExchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Declare queue
            _channel.QueueDeclare(
                queue: EmailNotificationQueues.EmailTypeCreatedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: EmailNotificationQueues.EmailTypeCreatedQueue,
                exchange: EmailNotificationQueues.EmailNotificationExchange,
                routingKey: EmailNotificationQueues.EmailTypeCreatedRoutingKey);

            // Set prefetch count
            _channel.BasicQos(0, 1, false);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                try
                {
                    await HandleMessageAsync(ea, cancellationToken);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing email type created message");
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue on error
                }
            };

            _channel.BasicConsume(
                queue: EmailNotificationQueues.EmailTypeCreatedQueue,
                autoAck: false,
                consumerTag: $"EmailTypeCreatedConsumer-{System.Diagnostics.Process.GetCurrentProcess().Id}",
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: _consumer);

            _logger.LogInformation("EmailTypeCreatedConsumer started, listening to queue: {QueueName}", 
                EmailNotificationQueues.EmailTypeCreatedQueue);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting EmailTypeCreatedConsumer");
            throw;
        }
    }

    /// <summary>
    /// Stops consuming events
    /// </summary>
    public async Task StopAsync()
    {
        try
        {
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
            }

            _logger.LogInformation("EmailTypeCreatedConsumer stopped");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping EmailTypeCreatedConsumer");
        }
    }

    /// <summary>
    /// Handles received email type created messages
    /// </summary>
    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(
                "Received EmailTypeCreated message from {Routing\nKey}: {Message}",
                ea.RoutingKey,
                message);

            // TODO: Parse and process the message
            // 1. Deserialize JSON message
            // 2. Extract email type information
            // 3. Perform business logic (send notifications, update external systems, etc.)
            // 4. Log audit trail

            // Example parsing (would need JSON deserialization):
            // var emailTypeInfo = JsonSerializer.Deserialize<EmailTypeCreatedMessage>(message);
            // await ProcessEmailTypeCreatedAsync(emailTypeInfo, cancellationToken);

            _logger.LogInformation("Successfully processed EmailTypeCreated message");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling EmailTypeCreated message");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        _channel?.Dispose();
    }
}

/// <summary>
/// Consumer for recipient added events
/// </summary>
public class RecipientAddedConsumer : IMessageConsumer, IAsyncDisposable
{
    private readonly IRabbitMqConnection _connection;
    private readonly ILogger<RecipientAddedConsumer> _logger;
    private IModel? _channel;
    private AsyncEventingBasicConsumer? _consumer;

    public RecipientAddedConsumer(IRabbitMqConnection connection, ILogger<RecipientAddedConsumer> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// Starts consuming recipient added events
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!_connection.IsConnected)
            {
                _connection.Connect();
            }

            _channel = _connection.Connection.CreateModel();

            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: EmailNotificationQueues.EmailNotificationExchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Declare queue
            _channel.QueueDeclare(
                queue: EmailNotificationQueues.RecipientAddedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: EmailNotificationQueues.RecipientAddedQueue,
                exchange: EmailNotificationQueues.EmailNotificationExchange,
                routingKey: EmailNotificationQueues.RecipientAddedRoutingKey);

            // Set prefetch count
            _channel.BasicQos(0, 1, false);

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (model, ea) =>
            {
                try
                {
                    await HandleMessageAsync(ea, cancellationToken);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing recipient added message");
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue on error
                }
            };

            _channel.BasicConsume(
                queue: EmailNotificationQueues.RecipientAddedQueue,
                autoAck: false,
                consumerTag: $"RecipientAddedConsumer-{System.Diagnostics.Process.GetCurrentProcess().Id}",
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: _consumer);

            _logger.LogInformation("RecipientAddedConsumer started, listening to queue: {QueueName}", 
                EmailNotificationQueues.RecipientAddedQueue);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting RecipientAddedConsumer");
            throw;
        }
    }

    /// <summary>
    /// Stops consuming events
    /// </summary>
    public async Task StopAsync()
    {
        try
        {
            if (_channel?.IsOpen == true)
            {
                _channel.Close();
            }

            _logger.LogInformation("RecipientAddedConsumer stopped");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping RecipientAddedConsumer");
        }
    }

    /// <summary>
    /// Handles received recipient added messages
    /// </summary>
    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(
                "Received RecipientAdded message from {RoutingKey}: {Message}",
                ea.RoutingKey,
                message);

            // TODO: Parse and process the message
            // 1. Deserialize JSON message
            // 2. Extract recipient information
            // 3. Perform business logic (send confirmation, update systems, etc.)
            // 4. Log audit trail

            _logger.LogInformation("Successfully processed RecipientAdded message");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling RecipientAdded message");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
        _channel?.Dispose();
    }
}
