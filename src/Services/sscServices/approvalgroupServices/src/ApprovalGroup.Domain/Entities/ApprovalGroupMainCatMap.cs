namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_MAINCATMAP table - AP Group Main Category Mapping
/// </summary>
public class ApprovalGroupMainCatMap : BaseEntity
{
    public long MapId { get; private set; }
    public long MapGroupMapId { get; private set; }
    public long MapMainCat { get; private set; }

    // Navigation
    public ApprovalGroupMap? ApprovalGroupMap { get; private set; }

    private ApprovalGroupMainCatMap() { }

    public static ApprovalGroupMainCatMap Create(long mapId, long groupMapId, long mainCat)
    {
        return new ApprovalGroupMainCatMap
        {
            MapId = mapId,
            MapGroupMapId = groupMapId,
            MapMainCat = mainCat
        };
    }
}
