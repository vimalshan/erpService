using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TeamServices.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase<T> : BackgroundService
{
    private readonly ILogger _logger;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;
    private readonly string _exchange;
    private readonly string _routingKey;

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger, string queueName, string exchange, string routingKey)
    {
        _logger = logger;
        _queueName = queueName;
        _exchange = exchange;
        _routingKey = routingKey;

        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = configuration["RabbitMQ:UserName"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672")
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry connecting in a loop so the app can start even without RabbitMQ
        while (!stoppingToken.IsCancellationRequested)
        {
            IConnection? connection = null;
            IChannel? channel = null;
            try
            {
                connection = await _factory.CreateConnectionAsync(stoppingToken);
                channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_queueName, _exchange, _routingKey, cancellationToken: stoppingToken);

                _logger.LogInformation("RabbitMQ consumer connected to queue {Queue}", _queueName);

                var consumer = new AsyncEventingBasicConsumer(channel);
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
                        await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message from queue {Queue}", _queueName);
                        await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                    }
                };

                await channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                // Keep alive until cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ consumer for queue {Queue} failed to connect. Retrying in 30s...", _queueName);
                if (channel is { IsOpen: true }) await channel.CloseAsync(stoppingToken);
                if (connection is { IsOpen: true }) await connection.CloseAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }

    protected abstract Task HandleMessageAsync(T message, CancellationToken cancellationToken);
}
