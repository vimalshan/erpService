using Polly;
using TdsService.Application.Common.Interfaces;

namespace TdsService.Infrastructure.Services;

/// <summary>
/// Wraps a real IMessagePublisher with a Polly resilience pipeline (circuit-breaker + retry).
/// </summary>
public sealed class PollyWrappedMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMqMessagePublisher _inner;
    private readonly ResiliencePipeline _pipeline;

    public PollyWrappedMessagePublisher(RabbitMqMessagePublisher inner, ResiliencePipeline pipeline)
    {
        _inner = inner;
        _pipeline = pipeline;
    }

    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
        => _pipeline.ExecuteAsync(
            async token => await _inner.PublishAsync(exchange, routingKey, message, token),
            ct).AsTask();

    public ValueTask DisposeAsync() => _inner.DisposeAsync();
}
