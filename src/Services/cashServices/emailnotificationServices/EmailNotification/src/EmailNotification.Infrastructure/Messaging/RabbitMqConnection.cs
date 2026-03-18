using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmailNotification.Infrastructure.Messaging;

/// <summary>
/// Configuration for RabbitMQ connection
/// </summary>
public class RabbitMqConfiguration
{
    /// <summary>
    /// Hostname of RabbitMQ server
    /// </summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Port of RabbitMQ server
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Username for RabbitMQ authentication
    /// </summary>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Password for RabbitMQ authentication
    /// </summary>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Virtual host on the RabbitMQ server
    /// </summary>
    public string VirtualHost { get; set; } = "/";
}

/// <summary>
/// RabbitMQ connection factory interface
/// </summary>
public interface IRabbitMqConnection
{
    /// <summary>
    /// Gets the RabbitMQ connection
    /// </summary>
    IConnection Connection { get; }

    /// <summary>
    /// Connects to RabbitMQ if not already connected
    /// </summary>
    void Connect();

    /// <summary>
    /// Disconnects from RabbitMQ
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Gets a value indicating whether the connection is established
    /// </summary>
    bool IsConnected { get; }
}

/// <summary>
/// Implementation of RabbitMQ connection
/// </summary>
public class RabbitMqConnection : IRabbitMqConnection
{
    private readonly RabbitMqConfiguration _configuration;
    private readonly ILogger<RabbitMqConnection> _logger;
    private IConnection? _connection;

    public IConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                Connect();
            }
            return _connection ?? throw new InvalidOperationException("Failed to establish RabbitMQ connection");
        }
    }

    public bool IsConnected => _connection != null && _connection.IsOpen;

    public RabbitMqConnection(RabbitMqConfiguration configuration, ILogger<RabbitMqConnection> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Connect()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration.HostName,
                Port = _configuration.Port,
                UserName = _configuration.UserName,
                Password = _configuration.Password,
                VirtualHost = _configuration.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };

            _connection = factory.CreateConnection();

            _logger.LogInformation(
                "Connected to RabbitMQ: {HostName}:{Port}/{VirtualHost}",
                _configuration.HostName,
                _configuration.Port,
                _configuration.VirtualHost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ at {HostName}:{Port}", 
                _configuration.HostName, _configuration.Port);
            throw;
        }
    }

    public void Disconnect()
    {
        try
        {
            if (_connection?.IsOpen == true)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                _logger.LogInformation("Disconnected from RabbitMQ");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while disconnecting from RabbitMQ");
        }
    }
}

/// <summary>
/// Consumer interface for RabbitMQ messages
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Starts consuming messages from the queue
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Stops consuming messages
    /// </summary>
    Task StopAsync();
}

/// <summary>
/// Message queue definitions for email notification domain
/// </summary>
public static class EmailNotificationQueues
{
    /// <summary>
    /// Exchange for email notification events
    /// </summary>
    public const string EmailNotificationExchange = "EmailNotification.Events";

    /// <summary>
    /// Queue for email type created events
    /// </summary>
    public const string EmailTypeCreatedQueue = "EmailNotification.EmailTypeCreated";

    /// <summary>
    /// Routing key for email type created events
    /// </summary>
    public const string EmailTypeCreatedRoutingKey = "email.type.created";

    /// <summary>
    /// Queue for recipient added events
    /// </summary>
    public const string RecipientAddedQueue = "EmailNotification.RecipientAdded";

    /// <summary>
    /// Routing key for recipient added events
    /// </summary>
    public const string RecipientAddedRoutingKey = "recipient.added";
}
