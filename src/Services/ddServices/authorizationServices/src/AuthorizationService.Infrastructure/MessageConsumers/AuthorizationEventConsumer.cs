using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Infrastructure.MessageConsumers;

public class AuthorizationEventConsumer
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthorizationEventConsumer> _logger;
    private bool _initialized = false;

    public AuthorizationEventConsumer(IConfiguration configuration, ILogger<AuthorizationEventConsumer> logger)
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
            _logger.LogInformation("Connected to RabbitMQ for authorization events");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ for authorization events. Listening will not work.");
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
                queue: "authorization.events",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnAuthorizationEventReceived;

            _channel.BasicConsume(
                queue: "authorization.events",
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("AuthorizationEventConsumer started listening on 'authorization.events' queue");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting to listen for authorization events");
        }
    }

    private void OnAuthorizationEventReceived(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received authorization event: {message}");

            ProcessAuthorizationEvent(message);

            _channel?.BasicAck(e.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing authorization event");
            _channel?.BasicNack(e.DeliveryTag, false, true);
        }
    }

    private void ProcessAuthorizationEvent(string message)
    {
        _logger.LogInformation($"Processed authorization event: {message}");
    }

    public void StopListening()
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
            _channel.Dispose();
        }
        if (_connection != null && _connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
