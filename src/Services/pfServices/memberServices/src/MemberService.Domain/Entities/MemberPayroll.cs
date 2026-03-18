using MemberService.Domain.Common;
using MemberService.Domain.Enums;

namespace MemberService.Domain.Entities;

public class MemberPayroll : BaseEntity
{
    public long MemberNo { get; private set; }
    public string UnitCode { get; private set; } = string.Empty;
    public long EmployeeNo { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public PayrollStatus Status { get; private set; } = PayrollStatus.Active;

    private MemberPayroll() { }

    public static MemberPayroll Create(long memberNo, string unitCode, long employeeNo, DateTime effectiveDate) =>
        new()
        {
            MemberNo = memberNo,
            UnitCode = unitCode,
            EmployeeNo = employeeNo,
            EffectiveDate = effectiveDate,
            Status = PayrollStatus.Active
        };

    public void Close(DateTime closureDate)
    {
        Status = PayrollStatus.Closed;
        ClosureDate = closureDate;
    }
}
