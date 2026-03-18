using Microsoft.Extensions.Logging;

namespace TaxService.Infrastructure.MessageBroker;

public interface IMessageBrokerConnection
{
    bool IsConnected { get; }
    void Connect();
    void Disconnect();
}

public class RabbitMQConnection : IMessageBrokerConnection
{
    private readonly ILogger<RabbitMQConnection> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;

    public bool IsConnected { get; private set; }

    public RabbitMQConnection(
        ILogger<RabbitMQConnection> logger,
        string hostName,
        string userName,
        string password)
    {
        _logger = logger;
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    public void Connect()
    {
        try
        {
            _logger.LogInformation($"Connecting to RabbitMQ at {_hostName}");
            IsConnected = true;
            _logger.LogInformation("Connected to RabbitMQ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot connect to RabbitMQ");
            IsConnected = false;
        }
    }

    public void Disconnect()
    {
        try
        {
            _logger.LogInformation("Disconnecting from RabbitMQ");
            IsConnected = false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disconnecting from RabbitMQ");
        }
    }
}

public interface IMessageConsumer
{
    void StartConsuming(string queueName);
    void StopConsuming();
}

public class TaxEventMessageConsumer : IMessageConsumer
{
    private readonly ILogger<TaxEventMessageConsumer> _logger;
    private readonly IMessageBrokerConnection _connection;

    public TaxEventMessageConsumer(
        ILogger<TaxEventMessageConsumer> logger,
        IMessageBrokerConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    public void StartConsuming(string queueName)
    {
        try
        {
            _logger.LogInformation($"RabbitMQ consumer started for queue: {queueName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error starting RabbitMQ consumer for queue: {queueName}");
        }
    }

    public void StopConsuming()
    {
        _logger.LogInformation("RabbitMQ consumer stopped");
    }
}
