using MobileAppManagement.Domain.Common;
using MobileAppManagement.Domain.Events;

namespace MobileAppManagement.Domain.Entities;

public class AppRegistration : AggregateRoot
{
    public long RegistrationId { get; private set; }
    public long? EmployeeSysId { get; private set; }
    public string? UserId { get; private set; }
    public long? UserSysId { get; private set; }
    public string? UserType { get; private set; }
    public long? PinNo { get; private set; }
    public DateTime? PinGeneratedOn { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public string? Status { get; private set; }
    public string? MobileNo { get; private set; }
    public string? ImeiNo { get; private set; }
    public string? Guid { get; private set; }
    public string? DeviceId { get; private set; }
    public string? DeviceType { get; private set; }

    private AppRegistration() { }

    public static AppRegistration Create(long registrationId, long? employeeSysId, string? userId,
        long? userSysId, string? userType, string? mobileNo, string? imeiNo, string? deviceId, string? deviceType)
    {
        var entity = new AppRegistration
        {
            RegistrationId = registrationId,
            EmployeeSysId = employeeSysId,
            UserId = userId,
            UserSysId = userSysId,
            UserType = userType,
            Status = "P",
            MobileNo = mobileNo,
            ImeiNo = imeiNo,
            DeviceId = deviceId,
            DeviceType = deviceType,
            UpdatedOn = DateTime.UtcNow
        };

        // Raise RegistrationCreatedEvent instead (event name should match state)
        // RegistrationCompletedEvent will be raised when status changes to 'R'
        // entity.AddDomainEvent(new RegistrationCreatedEvent(registrationId, userId ?? "", "P"));
        return entity;
    }

    public void GeneratePin(long pin)
    {
        PinNo = pin;
        PinGeneratedOn = DateTime.UtcNow;
        UpdatedOn = DateTime.UtcNow;
    }

    public void ChangeStatus(string newStatus)
    {
        var oldStatus = Status;
        Status = newStatus;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new RegistrationStatusChangedEvent(RegistrationId,
            oldStatus ?? "", newStatus));
    }

    public void MarkRegistered()
    {
        ChangeStatus("R");
    }

    public void Close()
    {
        ChangeStatus("C");
    }
}
