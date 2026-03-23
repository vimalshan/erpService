using RabbitMQ.Client;

namespace ReferenceService.Infrastructure.RabbitMQ;

/// <summary>
/// Configuration for RabbitMQ connection (bound from appsettings "RabbitMQ" section).
/// </summary>
public class RabbitMQConfiguration
{
    public string HostName    { get; set; } = "localhost";
    public int    Port        { get; set; } = 5672;
    public string UserName    { get; set; } = "guest";
    public string Password    { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
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
                    HostName               = _config.HostName,
                    Port                   = _config.Port,
                    UserName               = _config.UserName,
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

