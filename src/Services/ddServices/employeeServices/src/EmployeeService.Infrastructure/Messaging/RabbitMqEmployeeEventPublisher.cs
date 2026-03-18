using System.Text;
using System.Text.Json;
using EmployeeService.Application.Abstractions;
using EmployeeService.Shared.Messaging;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RabbitMQ.Client;

namespace EmployeeService.Infrastructure.Messaging;

public class RabbitMqEmployeeEventPublisher : IEmployeeEventPublisher
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqEmployeeEventPublisher> _logger;
    private readonly ResiliencePipeline _pipeline;

    public RabbitMqEmployeeEventPublisher(
        RabbitMqSettings settings,
        ILogger<RabbitMqEmployeeEventPublisher> logger)
    {
        _settings = settings;
        _logger = logger;
        _pipeline = BuildPipeline();
    }

    public async Task PublishAsync(EmployeeEventMessage message, CancellationToken cancellationToken = default)
    {
        try
        {
            await _pipeline.ExecuteAsync(async token =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    VirtualHost = _settings.VirtualHost
                };

                await using var connection = await factory.CreateConnectionAsync(token);
                await using var channel = await connection.CreateChannelAsync(cancellationToken: token);

                await channel.QueueDeclareAsync(
                    queue: _settings.EmployeeEventsQueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    passive: false,
                    noWait: false,
                    cancellationToken: token);

                var payload = JsonSerializer.SerializeToUtf8Bytes(message);
                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: _settings.EmployeeEventsQueueName,
                    mandatory: false,
                    body: payload,
                    cancellationToken: token);
            }, cancellationToken);

            _logger.LogInformation("Published employee event {EventType} for employee {EmployeeId} to queue {QueueName}", message.EventType, message.EmployeeId, _settings.EmployeeEventsQueueName);
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogWarning(ex, "RabbitMQ publish circuit is open. Skipping employee event {EventType} for employee {EmployeeId}.", message.EventType, message.EmployeeId);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish employee event {EventType} for employee {EmployeeId}. Continuing without blocking the request.", message.EventType, message.EmployeeId);
        }
    }

    private ResiliencePipeline BuildPipeline()
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            MaxRetryAttempts = _settings.PublishRetryCount,
            Delay = TimeSpan.FromSeconds(_settings.PublishRetryDelaySeconds),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            OnRetry = args =>
            {
                _logger.LogWarning(
                    args.Outcome.Exception,
                    "Retrying RabbitMQ publish attempt {Attempt} for employee event after {Delay}.",
                    args.AttemptNumber + 1,
                    args.RetryDelay);

                return ValueTask.CompletedTask;
            }
        };

        var circuitBreakerOptions = new CircuitBreakerStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            FailureRatio = _settings.PublishCircuitFailureRatio,
            SamplingDuration = TimeSpan.FromSeconds(Math.Max(_settings.PublishCircuitBreakDurationSeconds, 5)),
            MinimumThroughput = _settings.PublishCircuitMinimumThroughput,
            BreakDuration = TimeSpan.FromSeconds(_settings.PublishCircuitBreakDurationSeconds),
            OnOpened = args =>
            {
                _logger.LogWarning("RabbitMQ publish circuit opened for {BreakDuration}.", args.BreakDuration);
                return ValueTask.CompletedTask;
            },
            OnClosed = _ =>
            {
                _logger.LogInformation("RabbitMQ publish circuit closed.");
                return ValueTask.CompletedTask;
            },
            OnHalfOpened = _ =>
            {
                _logger.LogInformation("RabbitMQ publish circuit is half-open and testing recovery.");
                return ValueTask.CompletedTask;
            }
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .AddCircuitBreaker(circuitBreakerOptions)
            .Build();
    }
}