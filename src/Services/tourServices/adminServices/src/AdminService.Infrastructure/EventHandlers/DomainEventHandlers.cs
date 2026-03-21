using AdminService.Domain.Events;
using AdminService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Infrastructure.EventHandlers;

public class AdminMasterCreatedEventHandler : INotificationHandler<AdminMasterCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AdminMasterCreatedEventHandler> _logger;

    public AdminMasterCreatedEventHandler(IMessagePublisher publisher, ILogger<AdminMasterCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AdminMasterCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: AdminMasterCreated {AdminId}", notification.AdminId);
        await _publisher.PublishAsync("admin.events", "admin.master.created", notification, ct);
    }
}

public class AdminMasterUpdatedEventHandler : INotificationHandler<AdminMasterUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AdminMasterUpdatedEventHandler> _logger;

    public AdminMasterUpdatedEventHandler(IMessagePublisher publisher, ILogger<AdminMasterUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AdminMasterUpdatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: AdminMasterUpdated {AdminId}", notification.AdminId);
        await _publisher.PublishAsync("admin.events", "admin.master.updated", notification, ct);
    }
}

public class AccessRightsGrantedEventHandler : INotificationHandler<AccessRightsGrantedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AccessRightsGrantedEventHandler> _logger;

    public AccessRightsGrantedEventHandler(IMessagePublisher publisher, ILogger<AccessRightsGrantedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AccessRightsGrantedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: AccessRightsGranted {RightsId} for {UserId}", notification.RightsId, notification.UserId);
        await _publisher.PublishAsync("admin.events", "admin.access.granted", notification, ct);
    }
}

public class AccessRightsRevokedEventHandler : INotificationHandler<AccessRightsRevokedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AccessRightsRevokedEventHandler> _logger;

    public AccessRightsRevokedEventHandler(IMessagePublisher publisher, ILogger<AccessRightsRevokedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AccessRightsRevokedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: AccessRightsRevoked {RightsId}", notification.RightsId);
        await _publisher.PublishAsync("admin.events", "admin.access.revoked", notification, ct);
    }
}
