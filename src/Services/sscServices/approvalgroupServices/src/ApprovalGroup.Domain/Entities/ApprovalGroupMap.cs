namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_MAP table - AP Group Mapping
/// </summary>
public class ApprovalGroupMap : BaseEntity
{
    public long MapId { get; private set; }
    public long MapGroupId { get; private set; }
    public int MapPayBySpecific { get; private set; }
    public int MapBuSpecific { get; private set; }
    public long MapMainCat { get; private set; }
    public long MapSubCat { get; private set; }
    public char? MapCurrency { get; private set; }
    public long MapCreatedBy { get; private set; }
    public DateTime MapCreatedOn { get; private set; }
    public long? MapModifiedBy { get; private set; }
    public DateTime? MapModifiedOn { get; private set; }

    // Navigation
    public ApprovalGroupMaster? ApprovalGroup { get; private set; }
    public ICollection<ApprovalGroupUnitMap> UnitMaps { get; private set; } = new List<ApprovalGroupUnitMap>();
    public ICollection<ApprovalGroupPayBy> PayByMaps { get; private set; } = new List<ApprovalGroupPayBy>();
    public ICollection<ApprovalGroupMainCatMap> MainCatMaps { get; private set; } = new List<ApprovalGroupMainCatMap>();

    private ApprovalGroupMap() { }

    public static ApprovalGroupMap Create(long mapId, long groupId, int payBySpecific, int buSpecific,
        long mainCat, long subCat, long createdBy, char? currency = null)
    {
        return new ApprovalGroupMap
        {
            MapId = mapId,
            MapGroupId = groupId,
            MapPayBySpecific = payBySpecific,
            MapBuSpecific = buSpecific,
            MapMainCat = mainCat,
            MapSubCat = subCat,
            MapCurrency = currency,
            MapCreatedBy = createdBy,
            MapCreatedOn = DateTime.UtcNow
        };
    }

    public void Update(int payBySpecific, int buSpecific, long mainCat, long subCat, long modifiedBy, char? currency = null)
    {
        MapPayBySpecific = payBySpecific;
        MapBuSpecific = buSpecific;
        MapMainCat = mainCat;
        MapSubCat = subCat;
        MapCurrency = currency;
        MapModifiedBy = modifiedBy;
        MapModifiedOn = DateTime.UtcNow;
    }
}
