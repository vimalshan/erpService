using MassTransit;
using CardManagement.Application.Common.Interfaces;

namespace CardManagement.Infrastructure.Services;

public class MassTransitMessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;

    public MassTransitMessagePublisher(IBus bus) => _bus = bus;

    public async Task PublishAsync<T>(T message, string? routingKey = null, CancellationToken ct = default) where T : class
        => await _bus.Publish(message, ct);
}
