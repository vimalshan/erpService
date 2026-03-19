using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;
using MasterDataService.Domain.ValueObjects;

namespace MasterDataService.Domain.Entities;

public class ScannerMaster : AuditableEntity<long>
{
    public string? DeviceName { get; private set; }
    public long DeviceLocationId { get; private set; }
    public DevicePath DevicePath { get; private set; } = null!;

    private ScannerMaster() { }

    public static ScannerMaster Create(long id, string? deviceName, long deviceLocationId, string? devicePath)
    {
        var entity = new ScannerMaster
        {
            Id = id,
            DeviceName = deviceName,
            DeviceLocationId = deviceLocationId,
            DevicePath = DevicePath.Create(devicePath ?? string.Empty),
            CreatedAt = DateTime.UtcNow
        };

        entity.AddDomainEvent(new ScannerMasterCreatedEvent(entity.Id, entity.DeviceName));
        return entity;
    }

    public void Update(string? deviceName, long deviceLocationId, string? devicePath)
    {
        DeviceName = deviceName;
        DeviceLocationId = deviceLocationId;
        DevicePath = DevicePath.Create(devicePath ?? string.Empty);
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new ScannerMasterUpdatedEvent(Id, DeviceName));
    }
}
