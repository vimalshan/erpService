using UnitService.Domain.Events;
using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class EquipmentMaster : BaseEntity
{
    public int EquipmentId { get; private set; }
    public string EquipmentName { get; private set; } = string.Empty;
    public UnitCode UnitCode { get; private set; } = null!;
    public string Category { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public int LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public ICollection<EquipmentStatus> Statuses { get; private set; } = new List<EquipmentStatus>();

    private EquipmentMaster() { }

    public static EquipmentMaster Create(int equipmentId, string equipmentName, string unitCode, string category, int modifiedBy)
    {
        var equipment = new EquipmentMaster
        {
            EquipmentId = equipmentId,
            EquipmentName = equipmentName,
            UnitCode = UnitCode.From(unitCode),
            Category = category,
            StartDate = DateTime.UtcNow,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };

        equipment.AddDomainEvent(new EquipmentRegisteredEvent(equipmentId, equipmentName, unitCode));
        return equipment;
    }

    public void Update(string equipmentName, string category, int modifiedBy)
    {
        EquipmentName = equipmentName;
        Category = category;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Close(int modifiedBy)
    {
        CloseDate = DateTime.UtcNow;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
