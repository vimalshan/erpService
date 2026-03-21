using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProductService.Infrastructure.Services;

public sealed class ProductMessageConsumer : BackgroundService
{
    private readonly ILogger<ProductMessageConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public ProductMessageConsumer(IConfiguration configuration, ILogger<ProductMessageConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync("product.events", "topic", durable: true, cancellationToken: stoppingToken);
            var queueResult = await _channel.QueueDeclareAsync("product.consumer.queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(queueResult.QueueName, "product.events", "product.*", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("Received message [{RoutingKey}]: {Body}", ea.RoutingKey, body);

                // Process by routing key
                switch (ea.RoutingKey)
                {
                    case "product.created":
                        await HandleProductCreated(body);
                        break;
                    case "product.updated":
                        await HandleProductUpdated(body);
                        break;
                    case "product.deactivated":
                        await HandleProductDeactivated(body);
                        break;
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            await _channel.BasicConsumeAsync(queueResult.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            _logger.LogInformation("ProductMessageConsumer started listening on queue {Queue}", queueResult.QueueName);

            // Keep running until cancelled
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("ProductMessageConsumer stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ProductMessageConsumer");
        }
    }

    private Task HandleProductCreated(string body)
    {
        _logger.LogInformation("Processing product.created: {Body}", body);
        return Task.CompletedTask;
    }

    private Task HandleProductUpdated(string body)
    {
        _logger.LogInformation("Processing product.updated: {Body}", body);
        return Task.CompletedTask;
    }

    private Task HandleProductDeactivated(string body)
    {
        _logger.LogInformation("Processing product.deactivated: {Body}", body);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel?.IsOpen == true) await _channel.CloseAsync(ct);
        if (_connection?.IsOpen == true) await _connection.CloseAsync(ct);
        await base.StopAsync(ct);
    }
}
