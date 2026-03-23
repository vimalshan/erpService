using RabbitMQ.Client;

namespace ReferenceService.Infrastructure.RabbitMQ;

/// <summary>
/// Configuration for RabbitMQ connection.
/// </summary>
public class RabbitMQConfiguration
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

/// <summary>
/// RabbitMQ connection factory wrapper.
/// </summary>
public class RabbitMQConnectionFactory
{
    private readonly RabbitMQConfiguration _config;
    private IConnection? _connection;
    
    public RabbitMQConnectionFactory(RabbitMQConfiguration config)
    {
        _config = config;
    }
    
    public IConnection GetConnection()
    {
        if (_connection == null || _connection.IsOpen == false)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = _config.VirtualHost,
                AutomaticRecoveryEnabled = true
            };
            
            _connection = factory.CreateConnection();
        }
        
        return _connection;
    }
    
    public void Close()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}
