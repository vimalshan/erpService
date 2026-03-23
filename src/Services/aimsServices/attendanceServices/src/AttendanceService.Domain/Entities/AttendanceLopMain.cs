using AttendanceService.Domain.Common;
using AttendanceService.Domain.ValueObjects;

namespace AttendanceService.Domain.Entities;

public class AttendanceLopMain : BaseEntity
{
    public long LopEmpSysId { get; private set; }
    public long LopBatchId { get; private set; }
    public decimal LopDays { get; private set; }
    public LopType LopType { get; private set; } = default!;
    public long LopLastModifiedBy { get; private set; }
    public DateTime LopLastModifiedOn { get; private set; }

    public AttendanceBatch? Batch { get; private set; }

    private AttendanceLopMain() { }

    public static AttendanceLopMain Create(long id, long empSysId, long batchId,
        decimal lopDays, string lopType, long createdBy)
        => new()
        {
            Id = id,
            LopEmpSysId = empSysId,
            LopBatchId = batchId,
            LopDays = lopDays,
            LopType = LopType.From(lopType),
            LopLastModifiedBy = createdBy,
            LopLastModifiedOn = DateTime.UtcNow
        };
}
