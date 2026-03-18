namespace AccessService.Infrastructure.Messaging;

using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

/// <summary>
/// RabbitMQ connection factory for creating connections and channels
/// </summary>
public interface IRabbitMQConnectionFactory
{
    IConnection CreateConnection();
    IModel CreateChannel(IConnection connection);
}

/// <summary>
/// Default RabbitMQ connection factory implementation
/// </summary>
public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQConnectionFactory> _logger;

    public RabbitMQConnectionFactory(IConfiguration configuration, ILogger<RabbitMQConnectionFactory> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a RabbitMQ connection with configurable settings
    /// </summary>
    public IConnection CreateConnection()
    {
        var host = _configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672");
        var username = _configuration["RabbitMQ:Username"] ?? "guest";
        var password = _configuration["RabbitMQ:Password"] ?? "guest";
        var virtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/";

        var factory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
            UserName = username,
            Password = password,
            VirtualHost = virtualHost,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            RequestedHeartbeat = TimeSpan.FromSeconds(30),
            DispatchConsumersAsync = true
        };

        try
        {
            var connection = factory.CreateConnection();
            _logger.LogInformation($"RabbitMQ connection established to {host}:{port}");
            return connection;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create RabbitMQ connection: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Creates a channel from an existing connection
    /// </summary>
    public IModel CreateChannel(IConnection connection)
    {
        try
        {
            var channel = connection.CreateModel();
            channel.BasicQos(0, 1, false); // Fair dispatch - process one message at a time
            _logger.LogInformation("RabbitMQ channel created successfully");
            return channel;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to create RabbitMQ channel: {ex.Message}");
            throw;
        }
    }
}
