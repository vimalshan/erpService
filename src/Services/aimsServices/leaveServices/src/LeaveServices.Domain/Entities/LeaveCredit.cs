using LeaveServices.Domain.Common;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// LEAVE_CREDIT – Annual leave balance accrual per employee per leave type.
/// </summary>
public class LeaveCredit : AggregateRoot
{
    public long    CreditId             { get; private set; }
    public long    CreditEmpSysId       { get; private set; }
    public long    CreditLeaveId        { get; private set; }
    public char    CreditLeaveFlag      { get; private set; }
    public int     CreditYear           { get; private set; }
    public decimal CreditOpening        { get; private set; }
    public decimal CreditCredited       { get; private set; }
    public decimal CreditUtilized       { get; private set; }
    public decimal CreditClosing        { get; private set; }
    public long    CreditLastModifiedBy { get; private set; }
    public DateTime CreditLastModifiedOn { get; private set; }

    public LeaveMaster? LeaveMaster { get; private set; }

    private LeaveCredit() { }

    public static LeaveCredit Create(
        long creditId, long empSysId, long leaveId, char flag, int year,
        decimal opening, decimal credited, long modifiedBy)
    {
        return new LeaveCredit
        {
            CreditId             = creditId,
            Id                   = creditId,
            CreditEmpSysId       = empSysId,
            CreditLeaveId        = leaveId,
            CreditLeaveFlag      = flag,
            CreditYear           = year,
            CreditOpening        = opening,
            CreditCredited       = credited,
            CreditUtilized       = 0,
            CreditClosing        = opening + credited,
            CreditLastModifiedBy = modifiedBy,
            CreditLastModifiedOn = DateTime.UtcNow
        };
    }

    public void AddUtilization(decimal days, long modifiedBy)
    {
        CreditUtilized      += days;
        CreditClosing        = CreditOpening + CreditCredited - CreditUtilized;
        CreditLastModifiedBy = modifiedBy;
        CreditLastModifiedOn = DateTime.UtcNow;
    }

    public decimal AvailableBalance => CreditOpening + CreditCredited - CreditUtilized;
}
