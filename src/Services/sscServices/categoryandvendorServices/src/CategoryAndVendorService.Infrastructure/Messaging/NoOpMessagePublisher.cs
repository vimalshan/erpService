using CategoryAndVendorService.Application.Interfaces;

namespace CategoryAndVendorService.Infrastructure.Messaging;

public class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string queueName, T message, CancellationToken ct = default)
        => Task.CompletedTask;
}
