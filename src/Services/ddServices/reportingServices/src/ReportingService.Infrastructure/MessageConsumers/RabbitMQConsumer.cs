using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ReportingService.Infrastructure.MessageConsumers;

public interface IRabbitMQConsumer
{
    void ListenToQueue(string queueName, Func<string, Task> messageHandler);
    void PublishMessage(string queueName, object message);
}

public class RabbitMQConsumer : IRabbitMQConsumer
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;
    private readonly ILogger<RabbitMQConsumer>? _logger;
    private bool _connected = false;

    public RabbitMQConsumer(string hostname, string username = "guest", string password = "guest", ILogger<RabbitMQConsumer>? logger = null)
    {
        _hostname = hostname;
        _username = username;
        _password = password;
        _logger = logger;
    }

    private bool EnsureConnected()
    {
        if (_connected && _connection?.IsOpen == true)
            return true;

        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };
            _connection = factory.CreateConnection();
            _connected = true;
            _logger?.LogInformation("Connected to RabbitMQ at {Hostname}", _hostname);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to connect to RabbitMQ at {Hostname}. RabbitMQ features will be unavailable.", _hostname);
            _connected = false;
            return false;
        }
    }

    public void ListenToQueue(string queueName, Func<string, Task> messageHandler)
    {
        if (!EnsureConnected() || _connection == null)
        {
            _logger?.LogWarning("Cannot listen to queue {QueueName}: RabbitMQ not connected", queueName);
            return;
        }

        try
        {
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                try
                {
                    messageHandler(message).Wait();
                    _channel?.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception)
                {
                    _channel?.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queueName, false, consumer);
            _logger?.LogInformation("Started listening to queue {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error setting up queue listener for {QueueName}", queueName);
        }
    }

    public void PublishMessage(string queueName, object message)
    {
        if (!EnsureConnected() || _connection == null)
        {
            _logger?.LogWarning("Cannot publish message to queue {QueueName}: RabbitMQ not connected", queueName);
            return;
        }

        try
        {
            _channel ??= _connection.CreateModel();
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(string.Empty, queueName, null, body);
            _logger?.LogDebug("Published message to queue {QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error publishing message to queue {QueueName}", queueName);
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
