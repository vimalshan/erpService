using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AttendanceService.Infrastructure.EventBus.RabbitMQ;

public class RabbitMQConnection : IAsyncDisposable
{
    private IConnection? _connection;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQConnection> _logger;

    public RabbitMQConnection(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQConnection> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        if (_connection is null || !_connection.IsOpen)
            await ConnectAsync();

        return await _connection!.CreateChannelAsync();
    }

    private async Task ConnectAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync("AttendanceService");
        _logger.LogInformation("RabbitMQ connected to {Host}", _settings.Host);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}

public class RabbitMQSettings
{
    public const string Section = "RabbitMQ";
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 5672;
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public string VirtualHost { get; init; } = "/";
}
