using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace FinanceService.Infrastructure.Messaging;

public class RabbitMqConnection : IAsyncDisposable
{
    private readonly ILogger<RabbitMqConnection> _logger;
    private IConnection? _connection;
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqConnection(string hostName, string userName, string password, ILogger<RabbitMqConnection> logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };
    }

    public async Task<IConnection> GetConnectionAsync(CancellationToken ct = default)
    {
        if (_connection is { IsOpen: true })
            return _connection;

        _connection = await _connectionFactory.CreateConnectionAsync(ct);
        _logger.LogInformation("RabbitMQ connection established to {HostName}", _connectionFactory.HostName);
        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is { IsOpen: true })
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}
