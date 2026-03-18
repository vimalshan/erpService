using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class FlSwitchoverTime : Entity
{
    public int FillingLineId { get; private set; }
    public int FromMainProductId { get; private set; }
    public int ToMainProductId { get; private set; }
    public int TimeInHours { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    protected FlSwitchoverTime() { }

    public static FlSwitchoverTime Create(int lineId, int fromProductId, int toProductId, int hours, int createdBy)
    {
        if (hours <= 0)
            throw new ArgumentException("Switchover time in hours must be positive.");
        return new FlSwitchoverTime
        {
            FillingLineId = lineId,
            FromMainProductId = fromProductId,
            ToMainProductId = toProductId,
            TimeInHours = hours,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }
}
