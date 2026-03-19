using StrategicStock.Application.Interfaces;

namespace StrategicStock.Infrastructure.Services;

/// <summary>
/// No-op publisher used when RabbitMQ is unavailable.
/// </summary>
public sealed class NoOpRabbitMqPublisher : IRabbitMqPublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
        => Task.CompletedTask;
}
