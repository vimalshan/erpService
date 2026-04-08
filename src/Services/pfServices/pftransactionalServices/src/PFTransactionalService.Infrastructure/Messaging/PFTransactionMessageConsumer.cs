using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PFTransactionalService.Infrastructure.Messaging;

public class PFTransactionMessageConsumer : BackgroundService
{
    private readonly ILogger<PFTransactionMessageConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public PFTransactionMessageConsumer(IConfiguration configuration, ILogger<PFTransactionMessageConsumer> logger)
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

                await _channel.ExchangeDeclareAsync("pftransaction-exchange", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                var queueResult = await _channel.QueueDeclareAsync("pftransaction-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queueResult.QueueName, "pftransaction-exchange", "pftransaction.*", cancellationToken: stoppingToken);

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
                            case "pftransaction.accumulation.created":
                                await HandleAccumulationCreated(message);
                                break;
                            case "pftransaction.contribution.posted":
                                await HandleContributionPosted(message);
                                break;
                            case "pftransaction.withdrawal.processed":
                                await HandleWithdrawalProcessed(message);
                                break;
                            case "pftransaction.interest.applied":
                                await HandleInterestApplied(message);
                                break;
                            case "pftransaction.accumulation.closed":
                                await HandleAccumulationClosed(message);
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
                _logger.LogInformation("PF Transaction message consumer connected and listening");

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

    private Task HandleAccumulationCreated(string message)
    {
        _logger.LogInformation("Processing pftransaction.accumulation.created: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleContributionPosted(string message)
    {
        _logger.LogInformation("Processing pftransaction.contribution.posted: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleWithdrawalProcessed(string message)
    {
        _logger.LogInformation("Processing pftransaction.withdrawal.processed: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleInterestApplied(string message)
    {
        _logger.LogInformation("Processing pftransaction.interest.applied: {Message}", message);
        return Task.CompletedTask;
    }

    private Task HandleAccumulationClosed(string message)
    {
        _logger.LogInformation("Processing pftransaction.accumulation.closed: {Message}", message);
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
