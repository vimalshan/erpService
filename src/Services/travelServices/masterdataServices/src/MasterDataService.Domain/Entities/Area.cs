using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class Area : AuditableEntity
{
    public int AreaId { get; private set; }
    public string AreaName { get; private set; } = string.Empty;

    private Area() { }

    public Area(int areaId, string areaName)
    {
        AreaId = areaId;
        AreaName = areaName ?? throw new ArgumentNullException(nameof(areaName));
        AddDomainEvent(new AreaCreatedEvent(this));
    }

    public void UpdateName(string name)
    {
        AreaName = name ?? throw new ArgumentNullException(nameof(name));
    }
}
