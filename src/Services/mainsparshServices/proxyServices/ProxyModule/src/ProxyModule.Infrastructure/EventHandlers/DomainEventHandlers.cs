using MediatR;
using Microsoft.Extensions.Logging;
using ProxyModule.Domain.Events;
using ProxyModule.Infrastructure.Messaging;

namespace ProxyModule.Infrastructure.EventHandlers;

public class ProxyRightCreatedEventHandler : INotificationHandler<ProxyRightCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ProxyRightCreatedEventHandler> _logger;

    public ProxyRightCreatedEventHandler(IMessagePublisher publisher, ILogger<ProxyRightCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ProxyRightCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: ProxyRightCreated for ProxyId={ProxyId}", notification.ProxyId);
        await _publisher.PublishAsync("proxy-module-exchange", "proxy.right.created", notification, cancellationToken);
    }
}

public class ProxyRightDeactivatedEventHandler : INotificationHandler<ProxyRightDeactivatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ProxyRightDeactivatedEventHandler> _logger;

    public ProxyRightDeactivatedEventHandler(IMessagePublisher publisher, ILogger<ProxyRightDeactivatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ProxyRightDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: ProxyRightDeactivated for ProxyId={ProxyId}", notification.ProxyId);
        await _publisher.PublishAsync("proxy-module-exchange", "proxy.right.deactivated", notification, cancellationToken);
    }
}
