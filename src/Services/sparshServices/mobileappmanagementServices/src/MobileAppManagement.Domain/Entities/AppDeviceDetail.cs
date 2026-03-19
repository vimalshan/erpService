using MobileAppManagement.Domain.Common;
using MobileAppManagement.Domain.Events;

namespace MobileAppManagement.Domain.Entities;

public class AppDeviceDetail : AggregateRoot
{
    public decimal EmployeeSysId { get; private set; }
    public string DeviceId { get; private set; } = null!;
    public string Active { get; private set; } = null!;
    public string? DeviceType { get; private set; }
    public string? ImeiNo { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public decimal UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private AppDeviceDetail() { }

    public static AppDeviceDetail Create(decimal employeeSysId, string deviceId, string deviceType,
        string? imeiNo, decimal updatedBy)
    {
        var entity = new AppDeviceDetail
        {
            EmployeeSysId = employeeSysId,
            DeviceId = deviceId,
            Active = "Y",
            DeviceType = deviceType,
            ImeiNo = imeiNo,
            CreatedOn = DateTime.UtcNow,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new DeviceRegisteredEvent(employeeSysId, deviceId, deviceType));
        return entity;
    }

    public void UpdateDevice(string deviceType, string? imeiNo, decimal updatedBy)
    {
        DeviceType = deviceType;
        ImeiNo = imeiNo;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Active = "Y";

        AddDomainEvent(new DeviceRegisteredEvent(EmployeeSysId, DeviceId, deviceType));
    }

    public void Deactivate(decimal updatedBy)
    {
        Active = "N";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new DeviceDeactivatedEvent(EmployeeSysId, DeviceId));
    }

    public void Activate(decimal updatedBy)
    {
        Active = "Y";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
