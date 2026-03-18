using CompensationBenefits.Domain.Common;

namespace CompensationBenefits.Domain.Entities;

/// <summary>Maps to MOBILE_CONNECTION table</summary>
public class MobileConnection : BaseEntity
{
    public long ConnId { get; private set; }
    public long ConnEmpSysId { get; private set; }
    public DateTime ConnEffDate { get; private set; }
    public DateTime? ConnClsDate { get; private set; }
    public string ConnType { get; private set; } = default!;
    public long ConnPhoneNo { get; private set; }
    public string? ConnRemarks { get; private set; }
    public long? ConnOpenRequestNo { get; private set; }
    public long? ConnCloseRequestNo { get; private set; }
    public long ConnCalendarId { get; private set; }
    public long ConnCreatedBy { get; private set; }
    public DateTime ConnCreatedOn { get; private set; }
    public long? ConnModifiedBy { get; private set; }
    public DateTime? ConnModifiedOn { get; private set; }

    private MobileConnection() { }

    public static MobileConnection Create(long id, long empSysId, string type, long phoneNo,
        long calendarId, long createdBy, DateTime effDate)
    {
        return new MobileConnection
        {
            ConnId = id,
            ConnEmpSysId = empSysId,
            ConnType = type,
            ConnPhoneNo = phoneNo,
            ConnCalendarId = calendarId,
            ConnCreatedBy = createdBy,
            ConnCreatedOn = DateTime.UtcNow,
            ConnEffDate = effDate
        };
    }

    public void Close(long modifiedBy, DateTime closeDate)
    {
        ConnClsDate = closeDate;
        ConnModifiedBy = modifiedBy;
        ConnModifiedOn = DateTime.UtcNow;
    }
}

/// <summary>Maps to MOBILE_LIMITMAST table</summary>
public class MobileLimitMaster : BaseEntity
{
    public long LimitId { get; private set; }
    public long LimitOrg { get; private set; }
    public long LimitUnitId { get; private set; }
    public string LimitGradeCatId { get; private set; } = default!;
    public long LimitGradeId { get; private set; }
    public long LimitElgAmt { get; private set; }
    public DateTime LimitEffDate { get; private set; }
    public DateTime? LimitClsDate { get; private set; }
    public long LimitCreatedBy { get; private set; }
    public DateTime LimitCreatedOn { get; private set; }
    public long? LimitModifiedBy { get; private set; }
    public DateTime? LimitModifiedOn { get; private set; }

    private MobileLimitMaster() { }
}

/// <summary>Maps to MOBILE_ADDLIMIT table</summary>
public class MobileAdditionalLimit : BaseEntity
{
    public long AddId { get; private set; }
    public long AddEmpSysId { get; private set; }
    public DateTime AddEffDate { get; private set; }
    public DateTime? AddClsDate { get; private set; }
    public string? AddRemarks { get; private set; }
    public long AddAmt { get; private set; }
    public long AddCalendarId { get; private set; }
    public long AddCreatedBy { get; private set; }
    public DateTime AddCreatedOn { get; private set; }
    public decimal? AddModifiedBy { get; private set; }
    public DateTime? AddModifiedOn { get; private set; }

    private MobileAdditionalLimit() { }
}
