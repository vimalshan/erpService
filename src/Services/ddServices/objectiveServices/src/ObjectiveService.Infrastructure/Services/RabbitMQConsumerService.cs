using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ObjectiveService.Infrastructure.Services;

/// <summary>
/// Background service that consumes RabbitMQ messages for the Objective domain.
/// </summary>
public class RabbitMQConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMQConsumerService> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    // Queue / exchange constants
    private const string Exchange = "ObjectiveService.Exchange";
    private const string GoalCreatedQueue = "objective.goal.created";
    private const string ControlPointModifiedQueue = "objective.controlpoint.modified";
    private const string RoutingKeyGoalCreated = "goal.created";
    private const string RoutingKeyControlPointModified = "controlpoint.modified";

    public RabbitMQConsumerService(
        ILogger<RabbitMQConsumerService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var rabbitConfig = _configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"] ?? "localhost",
                Port = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672,
                UserName = rabbitConfig["UserName"] ?? "guest",
                Password = rabbitConfig["Password"] ?? "guest",
                VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
            };

            _connection = await factory.CreateConnectionAsync("ObjectiveService.Consumer", cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);

            // Declare and bind goal.created queue
            await _channel.QueueDeclareAsync(GoalCreatedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(GoalCreatedQueue, Exchange, RoutingKeyGoalCreated, cancellationToken: cancellationToken);

            // Declare and bind controlpoint.modified queue
            await _channel.QueueDeclareAsync(ControlPointModifiedQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(ControlPointModifiedQueue, Exchange, RoutingKeyControlPointModified, cancellationToken: cancellationToken);

            _logger.LogInformation("RabbitMQ consumer service started.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to RabbitMQ. Consumer service will not process messages.");
        }

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
        {
            _logger.LogWarning("RabbitMQ channel not available — consumer not running.");
            return;
        }

        var goalConsumer = new AsyncEventingBasicConsumer(_channel);
        goalConsumer.ReceivedAsync += async (_, ea) =>
        {
            await HandleMessageAsync(ea, HandleGoalCreatedAsync, stoppingToken);
        };

        var cpConsumer = new AsyncEventingBasicConsumer(_channel);
        cpConsumer.ReceivedAsync += async (_, ea) =>
        {
            await HandleMessageAsync(ea, HandleControlPointModifiedAsync, stoppingToken);
        };

        await _channel.BasicConsumeAsync(GoalCreatedQueue, autoAck: false, consumer: goalConsumer, cancellationToken: stoppingToken);
        await _channel.BasicConsumeAsync(ControlPointModifiedQueue, autoAck: false, consumer: cpConsumer, cancellationToken: stoppingToken);

        // Keep alive until cancellation
        await Task.Delay(Timeout.Infinite, stoppingToken).ContinueWith(_ => { }, CancellationToken.None);
    }

    private async Task HandleMessageAsync(
        BasicDeliverEventArgs ea,
        Func<string, CancellationToken, Task> handler,
        CancellationToken cancellationToken)
    {
        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
        try
        {
            _logger.LogInformation("Received message from {Exchange}/{RoutingKey}", ea.Exchange, ea.RoutingKey);
            await handler(body, cancellationToken);
            await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message from queue");
            // Nack without requeue after first failure to avoid infinite loop
            await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: cancellationToken);
        }
    }

    private Task HandleGoalCreatedAsync(string messageBody, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing GoalCreated message: {Body}", messageBody);
        var payload = JsonSerializer.Deserialize<GoalCreatedMessage>(messageBody);
        if (payload is null) return Task.CompletedTask;

        _logger.LogInformation(
            "GoalCreated — GoalId: {GoalId}, UserId: {UserId}, Period: {From} – {To}",
            payload.GoalId, payload.UserId, payload.PeriodFrom, payload.PeriodTo);

        // TODO: trigger notifications, update read models, etc.
        return Task.CompletedTask;
    }

    private Task HandleControlPointModifiedAsync(string messageBody, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing ControlPointModified message: {Body}", messageBody);
        var payload = JsonSerializer.Deserialize<ControlPointModifiedMessage>(messageBody);
        if (payload is null) return Task.CompletedTask;

        _logger.LogInformation(
            "ControlPointModified — CP Id: {Id}, Description: {Desc}",
            payload.ControlPointId, payload.Description);

        // TODO: update audit log, notify approvers, etc.
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync(cancellationToken);
        if (_connection is not null)
            await _connection.CloseAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
        _logger.LogInformation("RabbitMQ consumer service stopped.");
    }
}

// ── Message contracts ────────────────────────────────────────────────────────

public record GoalCreatedMessage(decimal GoalId, string UserId, DateTime PeriodFrom, DateTime PeriodTo);
public record ControlPointModifiedMessage(decimal ControlPointId, string Description);
