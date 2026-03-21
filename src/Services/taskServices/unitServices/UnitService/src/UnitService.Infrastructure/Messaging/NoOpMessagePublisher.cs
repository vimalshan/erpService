using UnitService.Application.Interfaces;

namespace UnitService.Infrastructure.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(T message, string exchangeName, string routingKey, CancellationToken ct = default)
        => Task.CompletedTask;
}
