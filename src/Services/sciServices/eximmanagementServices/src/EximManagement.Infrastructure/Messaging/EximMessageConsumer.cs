using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EximManagement.Infrastructure.Messaging;

/// <summary>Background service that consumes EXIM messages from RabbitMQ.</summary>
public class EximMessageConsumer : BackgroundService
{
    private readonly ILogger<EximMessageConsumer> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _exchange;
    private readonly string _queue;
    private IConnection? _connection;
    private IChannel? _channel;

    public EximMessageConsumer(IConfiguration configuration, ILogger<EximMessageConsumer> logger)
    {
        _logger = logger;
        _hostName = configuration["RabbitMQ:Host"] ?? "localhost";
        _userName = configuration["RabbitMQ:Username"] ?? "guest";
        _password = configuration["RabbitMQ:Password"] ?? "guest";
        _exchange = configuration["RabbitMQ:Exchange"] ?? "exim.exchange";
        _queue = configuration["RabbitMQ:Queue"] ?? "exim.queue";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EXIM Message Consumer starting...");

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(_queue, _exchange, "exim.#", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.Span);
                    _logger.LogInformation("Received EXIM message: RoutingKey={Key}, Body={Body}",
                        ea.RoutingKey, body);

                    await ProcessMessageAsync(ea.RoutingKey, body, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing EXIM message {DeliveryTag}", ea.DeliveryTag);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(_queue, autoAck: false, consumer, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("EXIM Message Consumer stopping.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EXIM Message Consumer encountered an error.");
        }
    }

    private Task ProcessMessageAsync(string routingKey, string body, CancellationToken ct)
    {
        switch (routingKey)
        {
            case "exim.file.uploaded":
                var fileMsg = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
                _logger.LogInformation("Processing file upload event: FileId={FileId}", fileMsg?["FileId"]);
                break;
            case "exim.data.processed":
                _logger.LogInformation("Processing data processed event.");
                break;
            default:
                _logger.LogWarning("Unknown routing key: {RoutingKey}", routingKey);
                break;
        }
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
