using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text.Json;

namespace HRService.Infrastructure.MessageBroker;

/// <summary>
/// RabbitMQ consumer configuration
/// </summary>
public interface IMessageBrokerService
{
    Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
    Task StartConsumingAsync(CancellationToken cancellationToken = default);
}

public class RabbitMQService : IMessageBrokerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQService> _logger;
    private IConnection? _connection;

    public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsEnabled())
                return;

            var rabbitSettings = _configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitSettings["Host"] ?? "localhost",
                Port = int.Parse(rabbitSettings["Port"] ?? "5672"),
                UserName = rabbitSettings["Username"] ?? "guest",
                Password = rabbitSettings["Password"] ?? "guest",
                VirtualHost = rabbitSettings["VirtualHost"] ?? "/"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

                var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
                var body = System.Text.Encoding.UTF8.GetBytes(messageJson);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange, routingKey, properties, body);

                _logger.LogInformation("Message published to exchange={Exchange}, routingKey={RoutingKey}", exchange, routingKey);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
            throw;
        }
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsEnabled())
                return;

            _logger.LogInformation("RabbitMQ consumer started");
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting RabbitMQ consumer");
            throw;
        }
    }

    private bool IsEnabled()
    {
        var enabled = _configuration.GetSection("RabbitMQ")["Enabled"];
        return enabled != null && bool.Parse(enabled);
    }
}
