using MassTransit;
using MediatR;
using SettingsService.Domain.Events;

namespace SettingsService.Infrastructure.Messaging;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    public UserCreatedEventHandler(IPublishEndpoint publishEndpoint) { _publishEndpoint = publishEndpoint; }
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(notification, cancellationToken);
}

public class UserDeactivatedEventHandler : INotificationHandler<UserDeactivatedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    public UserDeactivatedEventHandler(IPublishEndpoint publishEndpoint) { _publishEndpoint = publishEndpoint; }
    public async Task Handle(UserDeactivatedEvent notification, CancellationToken cancellationToken)
        => await _publishEndpoint.Publish(notification, cancellationToken);
}
