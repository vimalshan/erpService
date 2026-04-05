using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Entities;

/// <summary>Maps to TSACTIVITY_MASTER</summary>
public class TsActivity : AggregateRoot
{
    public long ActivityId => Id;
    public string ActivityName { get; private set; } = string.Empty;
    public string ActivityRole { get; private set; } = string.Empty;  // SD/TL/PM

    private TsActivity() { } // EF

    public TsActivity(long activityId, string activityName, string activityRole, long modifiedBy)
    {
        Id = activityId;
        ActivityName = activityName;
        ActivityRole = activityRole;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
