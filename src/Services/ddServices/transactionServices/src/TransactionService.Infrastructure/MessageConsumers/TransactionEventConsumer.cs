using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TransactionService.Infrastructure.MessageConsumers;

public class TransactionEventConsumer
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TransactionEventConsumer> _logger;
    private bool _initialized = false;

    public TransactionEventConsumer(IConfiguration configuration, ILogger<TransactionEventConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private bool EnsureConnected()
    {
        if (_initialized && _connection?.IsOpen == true)
            return true;

        try
        {
            var rabbitmqSettings = _configuration.GetSection("RabbitMQ");
            var hostname = rabbitmqSettings["Hostname"] ?? "localhost";
            var username = rabbitmqSettings["Username"] ?? "guest";
            var password = rabbitmqSettings["Password"] ?? "guest";

            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _initialized = true;
            _logger.LogInformation("Connected to RabbitMQ for transaction events");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ for transaction events. Listening will not work.");
            _initialized = true;
            return false;
        }
    }

    public void StartListening()
    {
        if (!EnsureConnected() || _connection == null)
        {
            _logger.LogWarning("Cannot start listening: RabbitMQ not connected");
            return;
        }

        try
        {
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "transaction.events",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnTransactionEventReceived;

            _channel.BasicConsume(
                queue: "transaction.events",
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("Started listening to transaction.events queue");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting up transaction event listener");
        }
    }

    private void OnTransactionEventReceived(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received transaction event: {Message}", message);
            ProcessTransactionEvent(message);

            _channel?.BasicAck(e.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing transaction event");
            _channel?.BasicNack(e.DeliveryTag, false, true);
        }
    }

    private void ProcessTransactionEvent(string message)
    {
        _logger.LogInformation("Processing transaction event: {Message}", message);
    }
}
