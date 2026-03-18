using MassTransit;
using Document.Application.Common.Interfaces;

namespace Document.Infrastructure.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;

    public MessagePublisher(IBus bus) => _bus = bus;

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        => await _bus.Publish(message, cancellationToken);
}
