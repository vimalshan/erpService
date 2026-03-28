using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using AuthorizationService.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Infrastructure.MessageConsumers;

public class DomainEventPublisher : IDomainEventPublisher
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DomainEventPublisher> _logger;
    private bool _initialized = false;

    public DomainEventPublisher(IConfiguration configuration, ILogger<DomainEventPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private bool EnsureConnected()
    {
        if (_initialized && _connection?.IsOpen == true)
            return true;

        try
        {
            var rabbitmqSettings = _configuration.GetSection("RabbitMQ");
            var hostname = rabbitmqSettings["Hostname"] ?? "localhost";
            var username = rabbitmqSettings["Username"] ?? "guest";
            var password = rabbitmqSettings["Password"] ?? "guest";

            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
            _initialized = true;
            _logger.LogInformation("Connected to RabbitMQ for event publishing");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ for event publishing. Events will not be published.");
            _initialized = true;
            return false;
        }
    }

    public Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default)
    {
        return PublishAsync(new[] { @event }, cancellationToken);
    }

    public Task PublishAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        if (!EnsureConnected() || _connection == null)
        {
            _logger.LogWarning("RabbitMQ not connected. Domain events will not be published.");
            return Task.CompletedTask;
        }

        try
        {
            _channel ??= _connection.CreateModel();

            var queueName = "authorization.domain.events";
            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            foreach (var @event in events)
            {
                var json = JsonSerializer.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: queueName,
                    body: body);

                _logger.LogInformation($"Published domain event: {@event.GetType().Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain events to RabbitMQ");
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
