using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CourseService.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "course.events";
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
}

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqPublisher> _logger;

    private RabbitMqPublisher(IConnection connection, IChannel channel, RabbitMqOptions options, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _channel = channel;
        _options = options;
        _logger = logger;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger)
    {
        var opt = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opt.Host,
            Port = opt.Port,
            UserName = opt.Username,
            Password = opt.Password,
            VirtualHost = opt.VirtualHost
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(opt.ExchangeName, ExchangeType.Topic, durable: true);

        return new RabbitMqPublisher(connection, channel, opt, logger);
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
        await _channel.BasicPublishAsync(_options.ExchangeName, routingKey, false, props, body, ct);
        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", _options.ExchangeName, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
