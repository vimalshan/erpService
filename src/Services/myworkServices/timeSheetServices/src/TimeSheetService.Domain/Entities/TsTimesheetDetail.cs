using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TSTIMESHEET_DET</summary>
public class TsTimesheetDetail : BaseEntity
{
    public long TimeId => Id;
    public long EmployeeSysId { get; private set; }
    public DateTime TimeDate { get; private set; }
    public long ProjectId { get; private set; }
    public long StageId { get; private set; }
    public long ActivityId { get; private set; }
    public long Hours { get; private set; }
    public string Remarks { get; private set; } = string.Empty;
    public long? ModuleId { get; private set; }
    public string? RefId { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private TsTimesheetDetail() { } // EF

    public TsTimesheetDetail(long timeId, long employeeSysId, DateTime timeDate,
        long projectId, long stageId, long activityId, long hours, string remarks,
        long? moduleId, string? refId, long createdBy)
    {
        Id = timeId;
        EmployeeSysId = employeeSysId;
        TimeDate = timeDate;
        ProjectId = projectId;
        StageId = stageId;
        ActivityId = activityId;
        Hours = hours;
        Remarks = remarks;
        ModuleId = moduleId;
        RefId = refId;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
        LastModifiedBy = createdBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
