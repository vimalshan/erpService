using DispatchPlanning.Domain.Common;

namespace DispatchPlanning.Domain.Entities;

public class DispatchPlanSubGroup : Entity
{
    public int SubGroupId { get; private set; }
    public int MainGroupId { get; private set; }
    public string SubGroupName { get; private set; } = default!;
    public int? ProductId { get; private set; }
    public int? SgDisplayOrder { get; private set; }
    public char CaptureTotalDirectly { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private DispatchPlanSubGroup() { }

    public static DispatchPlanSubGroup Create(int id, int mainGroupId, string name,
        int? productId, int? displayOrder, char captureTotalDirectly, int createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new DispatchPlanSubGroup
        {
            SubGroupId = id,
            MainGroupId = mainGroupId,
            SubGroupName = name,
            ProductId = productId,
            SgDisplayOrder = displayOrder,
            CaptureTotalDirectly = captureTotalDirectly,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }

    public void Update(string name, int? productId, int modifiedBy)
    {
        SubGroupName = name;
        ProductId = productId;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
