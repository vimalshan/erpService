using MediatR;
using Microsoft.Extensions.Logging;
using MobileAppManagement.Application.Interfaces;
using MobileAppManagement.Domain.Events;

namespace MobileAppManagement.Application.EventHandlers;

public class DeviceRegisteredEventHandler(
    ILogger<DeviceRegisteredEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<DeviceRegisteredEvent>
{
    public async Task Handle(DeviceRegisteredEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Device registered: Employee {EmpId}, Device {DeviceId}, Type {Type}",
            notification.EmployeeSysId, notification.DeviceId, notification.DeviceType);

        await publisher.PublishAsync("mobile-app", "device.registered", notification, ct);
    }
}

public class DeviceDeactivatedEventHandler(
    ILogger<DeviceDeactivatedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<DeviceDeactivatedEvent>
{
    public async Task Handle(DeviceDeactivatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Device deactivated: Employee {EmpId}, Device {DeviceId}",
            notification.EmployeeSysId, notification.DeviceId);

        await publisher.PublishAsync("mobile-app", "device.deactivated", notification, ct);
    }
}

public class UserLoggedInEventHandler(
    ILogger<UserLoggedInEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<UserLoggedInEvent>
{
    public async Task Handle(UserLoggedInEvent notification, CancellationToken ct)
    {
        logger.LogInformation("User logged in: LoginId {LoginId}, User {UserId}, Device {DeviceId}",
            notification.LoginId, notification.UserSysId, notification.DeviceId);

        await publisher.PublishAsync("mobile-app", "user.logged-in", notification, ct);
    }
}

public class RegistrationCompletedEventHandler(
    ILogger<RegistrationCompletedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<RegistrationCompletedEvent>
{
    public async Task Handle(RegistrationCompletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Registration completed: Id {RegId}, User {UserId}, Status {Status}",
            notification.RegistrationId, notification.UserId, notification.Status);

        await publisher.PublishAsync("mobile-app", "registration.completed", notification, ct);
    }
}

public class RegistrationStatusChangedEventHandler(
    ILogger<RegistrationStatusChangedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<RegistrationStatusChangedEvent>
{
    public async Task Handle(RegistrationStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Registration status changed: Id {RegId}, {OldStatus} -> {NewStatus}",
            notification.RegistrationId, notification.OldStatus, notification.NewStatus);

        await publisher.PublishAsync("mobile-app", "registration.status-changed", notification, ct);
    }
}
