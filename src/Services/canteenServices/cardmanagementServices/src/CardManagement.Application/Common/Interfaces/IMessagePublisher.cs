namespace CardManagement.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken ct = default) where T : class;
}
