using LookupService.Domain.Common;
using LookupService.Domain.Events;

namespace LookupService.Domain.Entities;

public class LovMaster : AggregateRoot
{
    public string? LovType { get; private set; }
    public long LovId { get; private set; }
    public string? LovName { get; private set; }

    // Navigation
    public LovTypeMaster? LovTypeMasterNavigation { get; private set; }
    public ICollection<LovUnitMap> UnitMappings { get; private set; } = [];
    public ICollection<LovPanelMap> PanelMappings { get; private set; } = [];

    private LovMaster() { }

    public static LovMaster Create(long lovId, string lovType, string lovName)
    {
        var lov = new LovMaster
        {
            LovId = lovId,
            LovType = lovType,
            LovName = lovName
        };

        lov.AddDomainEvent(new LovCreatedEvent(lovId, lovType, lovName));
        return lov;
    }

    public void UpdateName(string name)
    {
        var oldName = LovName;
        LovName = name;
        AddDomainEvent(new LovUpdatedEvent(LovId, oldName, name));
    }
}
