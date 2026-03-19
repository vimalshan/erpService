namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_UNITMAP table - AP Group Unit Mapping
/// </summary>
public class ApprovalGroupUnitMap : BaseEntity
{
    public long MapId { get; private set; }
    public long MapGroupMapId { get; private set; }
    public string MapBuId { get; private set; } = string.Empty;

    // Navigation
    public ApprovalGroupMap? ApprovalGroupMap { get; private set; }

    private ApprovalGroupUnitMap() { }

    public static ApprovalGroupUnitMap Create(long mapId, long groupMapId, string buId)
    {
        return new ApprovalGroupUnitMap
        {
            MapId = mapId,
            MapGroupMapId = groupMapId,
            MapBuId = buId
        };
    }
}
