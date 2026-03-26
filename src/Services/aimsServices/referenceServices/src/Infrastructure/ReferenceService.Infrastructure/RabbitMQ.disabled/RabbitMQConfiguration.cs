using RabbitMQ.Client;

namespace ReferenceService.Infrastructure.RabbitMQ;

/// <summary>
/// Configuration for RabbitMQ connection (bound from appsettings "RabbitMQ" section).
/// </summary>
public class RabbitMQConfiguration
{
    public string Host        { get; set; } = "localhost";
    public int    Port        { get; set; } = 5672;
    public string Username    { get; set; } = "guest";
    public string Password    { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "reference.events";
    public string LovTypeQueue { get; set; } = "reference.lovtype.updates";
}

/// <summary>
/// Async-safe RabbitMQ connection factory wrapper using RabbitMQ.Client 7.x async API.
/// </summary>
public sealed class RabbitMQConnectionFactory : IAsyncDisposable
{
    private readonly RabbitMQConfiguration _config;
    private IConnection? _connection;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMQConnectionFactory(RabbitMQConfiguration config)
    {
        _config = config;
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            if (_connection is null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory
                {
                    HostName               = _config.Host,
                    Port                   = _config.Port,
                    UserName               = _config.Username,
                    Password               = _config.Password,
                    VirtualHost            = _config.VirtualHost,
                    AutomaticRecoveryEnabled = true
                };
                _connection = await factory.CreateConnectionAsync(ct);
            }
            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
        _lock.Dispose();
    }
}

