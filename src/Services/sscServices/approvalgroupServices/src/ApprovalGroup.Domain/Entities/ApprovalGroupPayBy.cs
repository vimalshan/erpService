namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_PAYBY table - AP Group Pay By
/// </summary>
public class ApprovalGroupPayBy : BaseEntity
{
    public long MapId { get; private set; }
    public long MapGroupMapId { get; private set; }
    public int MapPayBy { get; private set; }

    // Navigation
    public ApprovalGroupMap? ApprovalGroupMap { get; private set; }

    private ApprovalGroupPayBy() { }

    public static ApprovalGroupPayBy Create(long mapId, long groupMapId, int payBy)
    {
        return new ApprovalGroupPayBy
        {
            MapId = mapId,
            MapGroupMapId = groupMapId,
            MapPayBy = payBy
        };
    }
}
