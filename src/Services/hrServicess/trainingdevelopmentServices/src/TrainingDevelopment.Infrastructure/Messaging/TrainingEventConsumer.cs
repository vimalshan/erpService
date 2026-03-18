using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TrainingDevelopment.Infrastructure.Messaging;

public class TrainingEventConsumer : BackgroundService
{
    private readonly ILogger<TrainingEventConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string QueueName = "training.events.queue";

    public TrainingEventConsumer(IConfiguration configuration, ILogger<TrainingEventConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
        };

        try
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(RabbitMQProducer.TrainingExchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(QueueName, RabbitMQProducer.TrainingExchange, "training.#", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Training Event Received: [{RoutingKey}] {Message}", ea.RoutingKey, message);

                try
                {
                    await ProcessMessageAsync(ea.RoutingKey, message, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process training event: {RoutingKey}", ea.RoutingKey);
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Training event consumer stopping.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Training event consumer encountered an error.");
        }
    }

    private Task ProcessMessageAsync(string routingKey, string message, CancellationToken ct)
    {
        switch (routingKey)
        {
            case "training.created":
                _logger.LogInformation("Processing Training Created event: {Message}", message);
                break;
            case "training.completed":
                _logger.LogInformation("Processing Training Completed event: {Message}", message);
                break;
            case "training.dropped":
                _logger.LogInformation("Processing Training Dropped event: {Message}", message);
                break;
            default:
                _logger.LogWarning("Unknown routing key: {RoutingKey}", routingKey);
                break;
        }
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
    }
}
