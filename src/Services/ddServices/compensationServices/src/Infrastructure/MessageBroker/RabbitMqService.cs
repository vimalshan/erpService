namespace CompensationService.Infrastructure.MessageBroker;

using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service for managing RabbitMQ connections and messaging.
/// </summary>
public interface IRabbitMqService
{
    /// <summary>Publishes a message to a  queue.</summary>
    Task PublishMessageAsync(string queueName, string message, CancellationToken cancellationToken = default);

    /// <summary>Subscribes to a queue.</summary>
    Task SubscribeToQueueAsync(string queueName, Func<string, Task> messageHandler, CancellationToken cancellationToken = default);
}

/// <summary>
/// RabbitMQ service implementation.
/// </summary>
public class RabbitMqService : IRabbitMqService
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(IConnection connection, ILogger<RabbitMqService> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishMessageAsync(string queueName, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var body = System.Text.Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
            _logger.LogInformation($"Message published to queue: {queueName}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error publishing message: {ex.Message}");
            throw;
        }
    }

    public async Task SubscribeToQueueAsync(string queueName, Func<string, Task> messageHandler, CancellationToken cancellationToken = default)
    {
        try
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                await messageHandler(message);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            _logger.LogInformation($"Subscribed to queue: {queueName}");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error subscribing to queue: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// Extension methods for RabbitMQ service registration.
/// </summary>
public static class RabbitMqServiceExtensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");
        var hostName = rabbitMqConfig["HostName"] ?? "localhost";
        var userName = rabbitMqConfig["UserName"] ?? "guest";
        var password = rabbitMqConfig["Password"] ?? "guest";
        var port = int.Parse(rabbitMqConfig["Port"] ?? "5672");

        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = port,
            VirtualHost = rabbitMqConfig["VirtualHost"] ?? "/"
        };

        try
        {
            var connection = factory.CreateConnection();
            services.AddSingleton(connection);
            services.AddScoped<IRabbitMqService, RabbitMqService>();
            Console.WriteLine("RabbitMQ connected successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RabbitMQ connection failed: {ex.Message}. Using mocked RabbitMQ service.");
            services.AddScoped<IRabbitMqService, MockRabbitMqService>();
        }

        return services;
    }
}

public class MockRabbitMqService : IRabbitMqService
{
    public Task PublishMessageAsync(string queueName, string message, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[MockRabbitMQ] Publish to '{queueName}': {message}");
        return Task.CompletedTask;
    }

    public Task SubscribeToQueueAsync(string queueName, Func<string, Task> messageHandler, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[MockRabbitMQ] Subscribed to '{queueName}'");
        return Task.CompletedTask;
    }
}
