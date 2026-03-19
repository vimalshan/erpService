using MediatR;

namespace MobileAppManagement.Domain.Events;

public record DeviceRegisteredEvent(decimal EmployeeSysId, string DeviceId, string DeviceType) : INotification;

public record DeviceDeactivatedEvent(decimal EmployeeSysId, string DeviceId) : INotification;

public record UserLoggedInEvent(decimal LoginId, decimal UserSysId, string DeviceId, DateTime LogonTime) : INotification;

public record RegistrationCompletedEvent(long RegistrationId, string UserId, string Status) : INotification;

public record RegistrationStatusChangedEvent(long RegistrationId, string OldStatus, string NewStatus) : INotification;
