using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Entities;

public class AttendanceNight : BaseEntity
{
    public long NightEmpSysId { get; private set; }
    public DateTime NightDate { get; private set; }
    public string NightNightType { get; private set; } = default!;
    public long NightLastModifiedBy { get; private set; }
    public DateTime NightLastModifiedOn { get; private set; }

    private AttendanceNight() { }

    public static AttendanceNight Create(long id, long empSysId, DateTime date,
        string nightType, long createdBy)
        => new()
        {
            Id = id,
            NightEmpSysId = empSysId,
            NightDate = date,
            NightNightType = nightType,
            NightLastModifiedBy = createdBy,
            NightLastModifiedOn = DateTime.UtcNow
        };
}
