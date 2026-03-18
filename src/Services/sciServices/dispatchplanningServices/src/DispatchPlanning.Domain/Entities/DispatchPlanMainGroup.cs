using DispatchPlanning.Domain.Common;
using DispatchPlanning.Domain.ValueObjects;

namespace DispatchPlanning.Domain.Entities;

public class DispatchPlanMainGroup : Entity
{
    public int MainGroupId { get; private set; }
    public string MainGroupName { get; private set; } = default!;
    public char GroupType { get; private set; }
    public char ProductSummary { get; private set; }
    public string TotalDisplayName { get; private set; } = default!;
    public int MgDisplayOrder { get; private set; }
    public int CompanyUnitId { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private readonly List<DispatchPlanSubGroup> _subGroups = new();
    public IReadOnlyCollection<DispatchPlanSubGroup> SubGroups => _subGroups.AsReadOnly();

    private DispatchPlanMainGroup() { }

    public static DispatchPlanMainGroup Create(int id, string name, char groupType, char productSummary,
        string totalDisplayName, int displayOrder, int companyUnitId, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new DispatchPlanMainGroup
        {
            MainGroupId = id,
            MainGroupName = name,
            GroupType = groupType,
            ProductSummary = productSummary,
            TotalDisplayName = totalDisplayName,
            MgDisplayOrder = displayOrder,
            CompanyUnitId = companyUnitId,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }

    public void Update(string name, char groupType, int modifiedBy)
    {
        MainGroupName = name;
        GroupType = groupType;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
