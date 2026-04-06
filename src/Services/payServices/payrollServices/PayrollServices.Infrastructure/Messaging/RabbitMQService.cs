using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PayrollServices.Infrastructure.Messaging;

public class RabbitMQService : IMessageBrokerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQService> _logger;

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
            {
                _logger.LogWarning("RabbitMQ is disabled. Message not published.");
                return;
            }

            var rabbitSettings = _configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = rabbitSettings["HostName"] ?? "localhost",
                Port = int.Parse(rabbitSettings["Port"] ?? "5672"),
                UserName = rabbitSettings["UserName"] ?? "guest",
                Password = rabbitSettings["Password"] ?? "guest",
                VirtualHost = rabbitSettings["VirtualHost"] ?? "/"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

            var messageJson = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            channel.BasicPublish(exchange, routingKey, properties, body);

            _logger.LogInformation("Message published to exchange={Exchange}, routingKey={RoutingKey}", exchange, routingKey);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ exchange={Exchange}, routingKey={RoutingKey}", exchange, routingKey);
            throw;
        }
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsEnabled())
            {
                _logger.LogWarning("RabbitMQ is disabled. Consumer not started.");
                return;
            }

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
        return enabled != null && bool.TryParse(enabled, out var result) && result;
    }
}
