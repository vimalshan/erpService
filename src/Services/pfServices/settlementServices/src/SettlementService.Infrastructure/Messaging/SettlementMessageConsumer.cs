using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SettlementService.Infrastructure.Messaging;

public class SettlementMessageConsumer : BackgroundService
{
    private readonly ILogger<SettlementMessageConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public SettlementMessageConsumer(IConfiguration configuration, ILogger<SettlementMessageConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
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
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("settlement-exchange", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                var queueResult = await _channel.QueueDeclareAsync("settlement-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queueResult.QueueName, "settlement-exchange", "settlement.*", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogInformation("Received message on {RoutingKey}: {Message}", ea.RoutingKey, message);

                        switch (ea.RoutingKey)
                        {
                            case "settlement.created":
                                await HandleSettlementCreated(message);
                                break;
                            case "settlement.approved":
                                await HandleSettlementApproved(message);
                                break;
                            case "settlement.completed":
                                await HandleSettlementCompleted(message);
                                break;
                            default:
                                _logger.LogWarning("Unknown routing key: {RoutingKey}", ea.RoutingKey);
                                break;
                        }

                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message");
                        await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                await _channel.BasicConsumeAsync(queueResult.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                _logger.LogInformation("Settlement message consumer connected and listening");

                // Keep running until connection drops or cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection failed. Retrying in 10 seconds...");
                _channel?.Dispose();
                _connection?.Dispose();
                _channel = null;
                _connection = null;
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    private Task HandleSettlementCreated(string message)
    {
        _logger.LogInformation("Processing settlement.created: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleSettlementApproved(string message)
    {
        _logger.LogInformation("Processing settlement.approved: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleSettlementCompleted(string message)
    {
        _logger.LogInformation("Processing settlement.completed: {Message}", message);
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
