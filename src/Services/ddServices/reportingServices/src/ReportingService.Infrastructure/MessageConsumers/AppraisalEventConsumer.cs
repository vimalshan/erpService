using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ReportingService.Infrastructure.MessageConsumers;

public class AppraisalEventConsumer
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppraisalEventConsumer> _logger;
    private bool _initialized = false;

    public AppraisalEventConsumer(IConfiguration configuration, ILogger<AppraisalEventConsumer> logger)
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
            _logger.LogInformation("Connected to RabbitMQ for appraisal events");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ for appraisal events. Listening will not work.");
            _initialized = true;  // Mark as initialized to not keep retrying
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

            // Declare queue for appraisal events
            _channel.QueueDeclare(
                queue: "appraisal.events",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnAppraisalEventReceived;

            _channel.BasicConsume(
                queue: "appraisal.events",
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("AppraisalEventConsumer started listening on 'appraisal.events' queue");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting to listen for appraisal events");
        }
    }

    private void OnAppraisalEventReceived(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received appraisal event: {message}");

            // Process the event
            ProcessAppraisalEvent(message);

            // Acknowledge the message
            _channel?.BasicAck(e.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing appraisal event");
            _channel?.BasicNack(e.DeliveryTag, false, true);
        }
    }

    private void ProcessAppraisalEvent(string message)
    {
        // Parse and process the event
        // This can be extended based on your event structure
        _logger.LogInformation($"Processed appraisal event: {message}");
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
