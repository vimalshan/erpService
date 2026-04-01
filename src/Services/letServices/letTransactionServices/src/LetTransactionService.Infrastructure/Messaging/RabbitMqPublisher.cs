using System.Text;
using System.Text.Json;
using LetTransactionService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LetTransactionService.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";
    public bool Enabled { get; set; } = true;
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string Exchange { get; set; } = "let.transaction.exchange";
}

public sealed class RabbitMqPublisher(
    IOptions<RabbitMqOptions> options,
    ILogger<RabbitMqPublisher> logger)
    : IMessagePublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMqOptions _opts = options.Value;

    private async Task EnsureChannelAsync()
    {
        if (_channel is { IsOpen: true }) return;

        var factory = new ConnectionFactory
        {
            HostName = _opts.Host,
            Port = _opts.Port,
            UserName = _opts.UserName,
            Password = _opts.Password,
            VirtualHost = _opts.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _opts.Exchange,
            type: ExchangeType.Topic,
            durable: true, autoDelete: false);
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        if (!_opts.Enabled)
        {
            logger.LogDebug("RabbitMQ is disabled; skipping publish to {RoutingKey}", routingKey);
            return;
        }

        await EnsureChannelAsync();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);
        var props = new BasicProperties { ContentType = "application/json", Persistent = true };

        await _channel!.BasicPublishAsync(
            exchange: _opts.Exchange, routingKey: routingKey,
            mandatory: false, basicProperties: props,
            body: body, cancellationToken: ct);

        logger.LogInformation("Published message to exchange={Exchange} routing={RoutingKey}", _opts.Exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}
