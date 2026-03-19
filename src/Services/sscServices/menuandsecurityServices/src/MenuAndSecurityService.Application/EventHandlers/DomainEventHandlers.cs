using MediatR;
using MenuAndSecurityService.Domain.Events;
using MenuAndSecurityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MenuAndSecurityService.Application.EventHandlers;

public class MenuCreatedEventHandler : INotificationHandler<MenuCreatedEvent>
{
    private readonly ILogger<MenuCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public MenuCreatedEventHandler(ILogger<MenuCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(MenuCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Menu created: {MenuId} - {MenuName}", notification.MenuId, notification.MenuName);
        await _publisher.PublishAsync("menu-exchange", "menu.created", notification, cancellationToken);
    }
}

public class MenuUpdatedEventHandler : INotificationHandler<MenuUpdatedEvent>
{
    private readonly ILogger<MenuUpdatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public MenuUpdatedEventHandler(ILogger<MenuUpdatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(MenuUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Menu updated: {MenuId} - {MenuName}", notification.MenuId, notification.MenuName);
        await _publisher.PublishAsync("menu-exchange", "menu.updated", notification, cancellationToken);
    }
}

public class MenuDeletedEventHandler : INotificationHandler<MenuDeletedEvent>
{
    private readonly ILogger<MenuDeletedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public MenuDeletedEventHandler(ILogger<MenuDeletedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(MenuDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Menu deleted: {MenuId}", notification.MenuId);
        await _publisher.PublishAsync("menu-exchange", "menu.deleted", notification, cancellationToken);
    }
}

public class MenuAccessGrantedEventHandler : INotificationHandler<MenuAccessGrantedEvent>
{
    private readonly ILogger<MenuAccessGrantedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public MenuAccessGrantedEventHandler(ILogger<MenuAccessGrantedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(MenuAccessGrantedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Access granted: AccessId={AccessId}, MenuId={MenuId}, RoleId={RoleId}",
            notification.AccessId, notification.MenuId, notification.RoleId);
        await _publisher.PublishAsync("menu-exchange", "menu.access.granted", notification, cancellationToken);
    }
}

public class MenuAccessRevokedEventHandler : INotificationHandler<MenuAccessRevokedEvent>
{
    private readonly ILogger<MenuAccessRevokedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public MenuAccessRevokedEventHandler(ILogger<MenuAccessRevokedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(MenuAccessRevokedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Access revoked: AccessId={AccessId}, MenuId={MenuId}, RoleId={RoleId}",
            notification.AccessId, notification.MenuId, notification.RoleId);
        await _publisher.PublishAsync("menu-exchange", "menu.access.revoked", notification, cancellationToken);
    }
}
