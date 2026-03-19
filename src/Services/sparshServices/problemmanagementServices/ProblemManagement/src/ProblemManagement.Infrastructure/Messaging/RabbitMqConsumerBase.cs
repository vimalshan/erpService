using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProblemManagement.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<TMessage> : BackgroundService where TMessage : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly string _queueName;
    private readonly string _exchange;
    private readonly string _routingKey;
    private IConnection? _connection;
    private IChannel? _channel;

    protected RabbitMqConsumerBase(
        IServiceProvider serviceProvider,
        ILogger logger,
        string queueName,
        string exchange,
        string routingKey)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _queueName = queueName;
        _exchange = exchange;
        _routingKey = routingKey;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = _serviceProvider.GetRequiredService<IConnectionFactory>();
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
                var body = Encoding.UTF8.GetString(ea.Body.Span);
                var message = JsonSerializer.Deserialize<TMessage>(body);
                if (message is not null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    await HandleMessageAsync(message, scope.ServiceProvider, stoppingToken);
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {Queue}", _queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    protected abstract Task HandleMessageAsync(TMessage message, IServiceProvider serviceProvider, CancellationToken ct);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
