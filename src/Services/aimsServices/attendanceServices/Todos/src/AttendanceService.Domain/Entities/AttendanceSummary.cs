using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Entities;

public class AttendanceSummary : BaseEntity
{
    public long SummaryEmpSysId { get; private set; }
    public long SummaryBatchId { get; private set; }
    public string SummaryAttType { get; private set; } = default!;
    public int SummaryDays { get; private set; }
    public long SummaryLastModifiedBy { get; private set; }
    public DateTime SummaryLastModifiedOn { get; private set; }

    public AttendanceBatch? Batch { get; private set; }

    private AttendanceSummary() { }

    public static AttendanceSummary Create(long id, long empSysId, long batchId,
        string attType, int days, long modifiedBy)
        => new()
        {
            Id = id,
            SummaryEmpSysId = empSysId,
            SummaryBatchId = batchId,
            SummaryAttType = attType,
            SummaryDays = days,
            SummaryLastModifiedBy = modifiedBy,
            SummaryLastModifiedOn = DateTime.UtcNow
        };
}
