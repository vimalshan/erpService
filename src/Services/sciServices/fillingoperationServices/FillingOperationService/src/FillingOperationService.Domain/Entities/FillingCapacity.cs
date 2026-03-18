using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class FillingCapacity : Entity
{
    public int FillingPointGroupId { get; private set; }
    public int MainProductId { get; private set; }
    public int PackageTypeId { get; private set; }
    public int ItemCapacityId { get; private set; }
    public int CapacityPerShift { get; private set; }
    public int UsagePriority { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    protected FillingCapacity() { }

    public static FillingCapacity Create(int groupId, int productId, int packageTypeId, int itemCapacityId, int capacityPerShift, int usagePriority, int createdBy)
    {
        if (capacityPerShift <= 0)
            throw new ArgumentException("Capacity per shift must be positive.");
        return new FillingCapacity
        {
            FillingPointGroupId = groupId,
            MainProductId = productId,
            PackageTypeId = packageTypeId,
            ItemCapacityId = itemCapacityId,
            CapacityPerShift = capacityPerShift,
            UsagePriority = usagePriority,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }
}
