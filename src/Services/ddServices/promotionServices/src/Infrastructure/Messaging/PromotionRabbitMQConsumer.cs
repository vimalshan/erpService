using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PromotionService.Features.Commands;

namespace PromotionService.Infrastructure.Messaging;

public class PromotionRabbitMQConsumer : BackgroundService
{
    private readonly ILogger<PromotionRabbitMQConsumer> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IModel? _channel;

    private const string ExchangeName = "promotion.events";
    private const string RatingFinalizedQueue = "rating.finalized";
    private const string PromotionApprovedQueue = "promotion.approved";
    private const string PromotionRejectedQueue = "promotion.rejected";
    private const string IncrementApprovedQueue = "increment.approved";

    public PromotionRabbitMQConsumer(
        ILogger<PromotionRabbitMQConsumer> logger,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting RabbitMQ consumer...");
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMq:HostName"] ?? "localhost",
                Port = int.TryParse(_configuration["RabbitMq:Port"], out var p) ? p : 5672,
                UserName = _configuration["RabbitMq:UserName"] ?? "guest",
                Password = _configuration["RabbitMq:Password"] ?? "guest",
                VirtualHost = _configuration["RabbitMq:VirtualHost"] ?? "/",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection("promotion-service-consumer");
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);

            DeclareAndBindQueue(RatingFinalizedQueue, "rating.finalized");
            DeclareAndBindQueue(PromotionApprovedQueue, "promotion.approved");
            DeclareAndBindQueue(PromotionRejectedQueue, "promotion.rejected");
            DeclareAndBindQueue(IncrementApprovedQueue, "increment.approved");

            _channel.BasicQos(0, 10, false);
            _logger.LogInformation("RabbitMQ consumer connected and queues declared.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ. Consumer will not start.");
        }

        return base.StartAsync(cancellationToken);
    }

    private void DeclareAndBindQueue(string queueName, string routingKey)
    {
        _channel!.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName, ExchangeName, routingKey);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel == null)
        {
            _logger.LogWarning("RabbitMQ channel not available. Skipping consumer registration.");
            return Task.CompletedTask;
        }

        RegisterConsumer<RatingFinalizedMessage>(RatingFinalizedQueue, HandleRatingFinalizedAsync, stoppingToken);
        RegisterConsumer<PromotionApprovedMessage>(PromotionApprovedQueue, HandlePromotionApprovedAsync, stoppingToken);
        RegisterConsumer<PromotionRejectedMessage>(PromotionRejectedQueue, HandlePromotionRejectedAsync, stoppingToken);
        RegisterConsumer<IncrementApprovedMessage>(IncrementApprovedQueue, HandleIncrementApprovedAsync, stoppingToken);

        return Task.CompletedTask;
    }

    private void RegisterConsumer<TMessage>(string queueName, Func<TMessage, CancellationToken, Task> handler, CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<TMessage>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (message != null)
                    await handler(message, stoppingToken);
                _channel!.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue {Queue}", queueName);
                _channel!.BasicNack(ea.DeliveryTag, false, requeue: false);
            }
        };
        _channel!.BasicConsume(queueName, autoAck: false, consumer);
        _logger.LogInformation("Consumer registered for queue: {Queue}", queueName);
    }

    private async Task HandleRatingFinalizedAsync(RatingFinalizedMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Received rating.finalized for EmployeeId={EmployeeId}, Year={Year}", message.EmployeeId, message.DDYear);
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        // Trigger downstream processing: e.g., auto-generate promotion recommendation if threshold met
        await Task.CompletedTask;
    }

    private async Task HandlePromotionApprovedAsync(PromotionApprovedMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Received promotion.approved for RecommendationId={Id}", message.RecommendationId);
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await Task.CompletedTask;
    }

    private async Task HandlePromotionRejectedAsync(PromotionRejectedMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Received promotion.rejected for RecommendationId={Id}", message.RecommendationId);
        await Task.CompletedTask;
    }

    private async Task HandleIncrementApprovedAsync(IncrementApprovedMessage message, CancellationToken ct)
    {
        _logger.LogInformation("Received increment.approved for RequestId={Id}", message.RequestId);
        await Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}

// ---- Message contracts (inbound) ----
public record RatingFinalizedMessage(decimal EmployeeId, int DDYear, string Grade);
public record PromotionApprovedMessage(decimal RecommendationId, decimal EmployeeId, string? PromotionType);
public record PromotionRejectedMessage(decimal RecommendationId, decimal EmployeeId, string? Reason);
public record IncrementApprovedMessage(decimal RequestId, decimal EmployeeId, decimal Amount);
