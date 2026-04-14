using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OrderService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<T> : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly string _queueName;
    private readonly string? _exchangeName;
    private readonly string? _routingKeyPattern;
    private IConnection? _connection;
    private IChannel? _channel;

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger, string queueName,
        string? exchangeName = null, string? routingKeyPattern = null)
    {
        _logger = logger;
        _configuration = configuration;
        _queueName = queueName;
        _exchangeName = exchangeName;
        _routingKeyPattern = routingKeyPattern;
    }

    protected abstract Task HandleMessageAsync(T message, CancellationToken cancellationToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                    UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                    Password = _configuration["RabbitMQ:Password"] ?? "guest",
                    Port = int.TryParse(_configuration["RabbitMQ:Port"], out var port) ? port : 5672
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

                if (_exchangeName != null)
                {
                    await _channel.ExchangeDeclareAsync(_exchangeName, "topic", durable: true, cancellationToken: stoppingToken);
                    await _channel.QueueBindAsync(_queueName, _exchangeName, _routingKeyPattern ?? "#", cancellationToken: stoppingToken);
                }

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var message = JsonSerializer.Deserialize<T>(body);

                        if (message != null)
                        {
                            await HandleMessageAsync(message, stoppingToken);
                        }

                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message from {Queue}", _queueName);
                        await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                _logger.LogInformation("Started consuming from queue: {Queue}", _queueName);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed for queue {Queue}. Retrying in 10 seconds...", _queueName);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync(cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
