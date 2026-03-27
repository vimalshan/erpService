using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TransactionProcessing.Infrastructure.Messaging.Settings;

namespace TransactionProcessing.Infrastructure.Messaging.RabbitMQ;

public sealed class TransactionEventConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TransactionEventConsumer> _logger;

    public TransactionEventConsumer(
        IOptions<RabbitMqSettings> settings,
        IServiceScopeFactory scopeFactory,
        ILogger<TransactionEventConsumer> logger)
    {
        _settings = settings.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.ExchangeDeclareAsync(_settings.ExchangeName, "topic", true, false, cancellationToken: stoppingToken);
            await channel.QueueDeclareAsync(_settings.QueueName, true, false, false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(_settings.QueueName, _settings.ExchangeName, "transaction.#", cancellationToken: stoppingToken);
            await channel.BasicQosAsync(0, 10, false, stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received message on {RoutingKey}: {Body}", ea.RoutingKey, body);

                    using var scope = _scopeFactory.CreateScope();
                    await ProcessMessageAsync(ea.RoutingKey, body, scope.ServiceProvider);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message {DeliveryTag}", ea.DeliveryTag);
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await channel.BasicConsumeAsync(_settings.QueueName, false, consumer, stoppingToken);
            _logger.LogInformation("Transaction event consumer started on queue {Queue}", _settings.QueueName);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Transaction event consumer stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction event consumer failed");
        }
    }

    private Task ProcessMessageAsync(string routingKey, string body, IServiceProvider services)
    {
        _logger.LogInformation("Processing {RoutingKey} message", routingKey);
        return Task.CompletedTask;
    }
}
