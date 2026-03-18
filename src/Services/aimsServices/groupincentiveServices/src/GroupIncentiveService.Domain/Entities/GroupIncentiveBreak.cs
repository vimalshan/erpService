using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.Entities;

public class GroupIncentiveBreak : BaseEntity
{
    public int GrpIncBrkId { get; private set; }
    public int GrpIncBrkGroupId { get; private set; }
    public decimal GrpIncBrkAttPercentage { get; private set; }
    public decimal GrpIncBrkIncPercentage { get; private set; }
    public DateTime GrpIncBrkEffDate { get; private set; }
    public DateTime? GrpIncBrkClsDate { get; private set; }
    public long GrpIncBrkLastModifiedBy { get; private set; }
    public DateTime GrpIncBrkLastModifiedOn { get; private set; }

    public GroupMaster? Group { get; private set; }

    private GroupIncentiveBreak() { }

    public static GroupIncentiveBreak Create(int id, int groupId, decimal attPercentage,
        decimal incPercentage, DateTime effDate, long createdBy)
    {
        if (attPercentage < 0 || attPercentage > 100)
            throw new DomainException("Attendance percentage must be between 0 and 100.");
        if (incPercentage < 0 || incPercentage > 100)
            throw new DomainException("Incentive percentage must be between 0 and 100.");

        return new GroupIncentiveBreak
        {
            GrpIncBrkId = id,
            GrpIncBrkGroupId = groupId,
            GrpIncBrkAttPercentage = attPercentage,
            GrpIncBrkIncPercentage = incPercentage,
            GrpIncBrkEffDate = effDate,
            GrpIncBrkLastModifiedBy = createdBy,
            GrpIncBrkLastModifiedOn = DateTime.UtcNow
        };
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        GrpIncBrkClsDate = closeDate;
        GrpIncBrkLastModifiedBy = modifiedBy;
        GrpIncBrkLastModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive => !GrpIncBrkClsDate.HasValue || GrpIncBrkClsDate > DateTime.UtcNow;
}
