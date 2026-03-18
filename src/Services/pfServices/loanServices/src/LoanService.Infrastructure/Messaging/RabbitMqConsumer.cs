using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace LoanService.Infrastructure.Messaging;

public abstract class RabbitMqConsumer<TMessage> : BackgroundService
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _queueName;
    private readonly string _exchange;
    private readonly string _routingKey;
    private readonly ILogger _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    protected RabbitMqConsumer(string hostName, string userName, string password,
        string queueName, string exchange, string routingKey, ILogger logger)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
        _queueName = queueName;
        _exchange = exchange;
        _routingKey = routingKey;
        _logger = logger;
    }

    protected abstract Task HandleMessageAsync(TMessage message, CancellationToken ct);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = _hostName, UserName = _userName, Password = _password };
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

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
                    await HandleMessageAsync(message, stoppingToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from {Queue}", _queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(_queueName, false, consumer, stoppingToken);

        // Keep running until cancelled
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel != null) { await _channel.CloseAsync(ct); _channel.Dispose(); }
        if (_connection != null) { await _connection.CloseAsync(ct); _connection.Dispose(); }
        await base.StopAsync(ct);
    }
}
