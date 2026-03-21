namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.ValueObjects;

public sealed class DeptApprover : Entity
{
    public long LocationId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public long DeptId { get; private set; }
    public long EmpSysId { get; private set; }
    public ApproverType Type { get; private set; } = null!;
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private DeptApprover() { }

    public static DeptApprover Create(
        long locationId, string unitCode, long deptId, long empSysId,
        string type, DateTime effectiveDate, long updatedBy)
    {
        return new DeptApprover
        {
            LocationId = locationId,
            UnitCode = new UnitCode(unitCode),
            DeptId = deptId,
            EmpSysId = empSysId,
            Type = new ApproverType(type),
            EffectiveDate = effectiveDate,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void Close(DateTime closureDate, long updatedBy)
    {
        ClosureDate = closureDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
