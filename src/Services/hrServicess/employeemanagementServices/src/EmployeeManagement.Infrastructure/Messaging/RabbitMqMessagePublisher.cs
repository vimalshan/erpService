using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EmployeeManagement.Infrastructure.Messaging;

public sealed class RabbitMqMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection? _connection;
    private readonly IChannel? _channel;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private readonly ResiliencePipeline _pipeline;

    public RabbitMqMessagePublisher(IConnection? connection, IChannel? channel,
        ILogger<RabbitMqMessagePublisher> logger)
    {
        _connection = connection;
        _channel = channel;
        _logger = logger;

        _pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(60),
                OnOpened = args =>
                {
                    _logger.LogWarning("RabbitMQ Circuit Breaker OPENED");
                    return ValueTask.CompletedTask;
                }
            })
            .AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential
            })
            .Build();
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
        where T : class
    {
        if (_channel is null)
        {
            _logger.LogWarning("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        await _pipeline.ExecuteAsync(async token =>
        {
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: token);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true };
            await _channel.BasicPublishAsync(exchange, routingKey, mandatory: false, props, body, token);
            _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }, ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
