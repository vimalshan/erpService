using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace InsuranceManagement.Infrastructure.MessageConsumers;

/// <summary>
/// RabbitMQ connection factory wrapper
/// </summary>
public interface IRabbitMqConnectionFactory
{
    IConnection CreateConnection();
}

/// <summary>
/// RabbitMQ connection factory implementation
/// </summary>
public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
{
    private readonly RabbitMqConfiguration _configuration;
    private readonly ILogger<RabbitMqConnectionFactory> _logger;

    public RabbitMqConnectionFactory(RabbitMqConfiguration configuration, ILogger<RabbitMqConnectionFactory> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IConnection CreateConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password = _configuration.Password,
                VirtualHost = _configuration.VirtualHost ?? "/",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var connection = factory.CreateConnection();
            _logger.LogInformation("RabbitMQ connection established successfully");
            return connection;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create RabbitMQ connection: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// RabbitMQ configuration
/// </summary>
public class RabbitMqConfiguration
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

/// <summary>
/// Generic RabbitMQ consumer for domain events
/// </summary>
public abstract class RabbitMqConsumer<T> : BaseMessageConsumer where T : class
{
    protected readonly IRabbitMqConnectionFactory _connectionFactory;
    protected IConnection? _connection;
    protected IModel? _channel;
    protected readonly string _queueName;
    protected readonly string _exchangeName;
    protected readonly string _routingKey;

    protected RabbitMqConsumer(
        IRabbitMqConnectionFactory connectionFactory,
        ILogger<RabbitMqConsumer<T>> logger,
        string queueName,
        string exchangeName,
        string routingKey)
        : base(logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
        _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        _routingKey = routingKey ?? throw new ArgumentNullException(nameof(routingKey));
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            // Declare queue
            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // Bind queue to exchange
            _channel.QueueBind(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: _routingKey);

            // Set QoS
            _channel.BasicQos(0, 1, false);

            LogMessage($"RabbitMQ consumer started for queue: {_queueName}");

            // Start consuming
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) => await HandleMessageAsync(ea);

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumerTag: $"{_queueName}-consumer",
                consumer: consumer);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            LogMessage($"Error starting RabbitMQ consumer: {ex.Message}", LogLevel.Error);
            throw;
        }
    }

    public override async Task StopAsync()
    {
        try
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }

            LogMessage($"RabbitMQ consumer stopped for queue: {_queueName}");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            LogMessage($"Error stopping RabbitMQ consumer: {ex.Message}", LogLevel.Error);
        }
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var messageObject = JsonSerializer.Deserialize<T>(message);

            if (messageObject != null)
            {
                await ProcessMessageAsync(messageObject);
                _channel!.BasicAck(ea.DeliveryTag, false);
            }
            else
            {
                LogMessage($"Failed to deserialize message from queue {_queueName}", LogLevel.Warning);
                _channel!.BasicNack(ea.DeliveryTag, false, true);
            }
        }
        catch (Exception ex)
        {
            LogMessage($"Error processing message from queue {_queueName}: {ex.Message}", LogLevel.Error);
            _channel!.BasicNack(ea.DeliveryTag, false, true);
        }
    }

    /// <summary>
    /// Process the message
    /// </summary>
    protected abstract Task ProcessMessageAsync(T message);
}
