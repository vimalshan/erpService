using UnitService.Domain.Entities;

namespace UnitService.Domain.Aggregates;

public class EquipmentAggregate
{
    public EquipmentMaster Equipment { get; }
    public IReadOnlyCollection<EquipmentStatus> StatusHistory { get; }

    public EquipmentAggregate(EquipmentMaster equipment, IEnumerable<EquipmentStatus> statuses)
    {
        Equipment = equipment;
        StatusHistory = statuses.OrderByDescending(s => s.StartDate).ToList().AsReadOnly();
    }

    public EquipmentStatus? LatestStatus => StatusHistory.FirstOrDefault();
    public bool IsActive => Equipment.CloseDate is null;
}
