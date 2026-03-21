using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ExpenseService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<TMessage> : BackgroundService
{
    private readonly ILogger _logger;
    private readonly string _queueName;
    private readonly string _exchange;
    private readonly string _routingKey;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;

    protected RabbitMqConsumerBase(
        IConfiguration configuration,
        ILogger logger,
        string queueName,
        string exchange,
        string routingKey)
    {
        _configuration = configuration;
        _logger = logger;
        _queueName = queueName;
        _exchange = exchange;
        _routingKey = routingKey;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _connection = await factory.CreateConnectionAsync(stoppingToken);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ not reachable for queue {Queue}. Retrying in 10s...", _queueName);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        if (stoppingToken.IsCancellationRequested) return;

        _channel = await _connection!.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(_queueName, _exchange, _routingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<TMessage>(body);
                if (message != null)
                {
                    await HandleMessageAsync(message, stoppingToken);
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from {Queue}", _queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(_queueName, false, consumer, stoppingToken);
        _logger.LogInformation("Started consuming from queue: {Queue}", _queueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    protected abstract Task HandleMessageAsync(TMessage message, CancellationToken ct);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel?.IsOpen == true)
            await _channel.CloseAsync(cancellationToken);
        if (_connection?.IsOpen == true)
            await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
