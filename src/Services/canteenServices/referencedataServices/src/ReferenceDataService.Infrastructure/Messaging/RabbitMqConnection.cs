using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ReferenceDataService.Infrastructure.Messaging;

public class RabbitMqConnection : IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqConnection> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConnection(IConfiguration configuration, ILogger<RabbitMqConnection> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IChannel> GetChannelAsync()
    {
        if (_channel != null && _connection != null && _connection.IsOpen)
            return _channel;

        // Dispose old resources
        if (_channel != null) { try { await _channel.CloseAsync(); } catch { } _channel = null; }
        if (_connection != null) { try { await _connection.CloseAsync(); } catch { } _connection = null; }

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        _logger.LogInformation("RabbitMQ connection established");

        return _channel;
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
            await _channel.CloseAsync();
        if (_connection != null)
            await _connection.CloseAsync();
    }
}
