using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Entities;

public class SwipeRawPunchLog : BaseEntity
{
    public long SwipeEmpSysId { get; private set; }
    public DateTime SwipePunchTime { get; private set; }
    public string SwipeGateNo { get; private set; } = default!;
    public string SwipePunchStatus { get; private set; } = default!;
    public string? SwipePullStatus { get; private set; }
    public DateTime LogCreatedOn { get; private set; }
    public long? LogCreatedBy { get; private set; }

    private SwipeRawPunchLog() { }

    public static SwipeRawPunchLog CreateFrom(SwipeRawPunch punch, long? createdBy)
        => new()
        {
            Id = punch.Id,
            SwipeEmpSysId = punch.SwipeEmpSysId,
            SwipePunchTime = punch.SwipePunchTime,
            SwipeGateNo = punch.SwipeGateNo,
            SwipePunchStatus = punch.SwipePunchStatus.Value,
            SwipePullStatus = punch.SwipePullStatus,
            LogCreatedOn = DateTime.UtcNow,
            LogCreatedBy = createdBy
        };
}
