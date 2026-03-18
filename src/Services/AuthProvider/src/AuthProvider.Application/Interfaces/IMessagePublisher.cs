namespace AuthProvider.Application.Interfaces;

/// <summary>Message publisher abstraction – decouples application from RabbitMQ/bus details.</summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
