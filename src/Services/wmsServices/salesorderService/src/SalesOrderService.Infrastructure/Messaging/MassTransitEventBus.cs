using MassTransit;
using SalesOrderService.Domain.Interfaces;

namespace SalesOrderService.Infrastructure.Messaging;

/// <summary>MassTransit-backed implementation of IEventBus.</summary>
public sealed class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class =>
        publishEndpoint.Publish(message, ct);
}
