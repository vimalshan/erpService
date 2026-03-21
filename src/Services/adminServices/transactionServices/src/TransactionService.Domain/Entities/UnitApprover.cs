namespace TransactionService.Domain.Entities;

using TransactionService.Domain.Common;
using TransactionService.Domain.ValueObjects;

public sealed class UnitApprover : Entity
{
    public long LocationId { get; private set; }
    public UnitCode UnitCode { get; private set; } = null!;
    public long EmpSysId { get; private set; }
    public ApproverType Type { get; private set; } = null!;
    public DateTime EffectiveDate { get; private set; }
    public string? ClosureDate { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private UnitApprover() { }

    public static UnitApprover Create(
        long locationId, string unitCode, long empSysId,
        string type, DateTime effectiveDate, long updatedBy)
    {
        return new UnitApprover
        {
            LocationId = locationId,
            UnitCode = new UnitCode(unitCode),
            EmpSysId = empSysId,
            Type = new ApproverType(type),
            EffectiveDate = effectiveDate,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }

    public void Close(string closureDate, long updatedBy)
    {
        ClosureDate = closureDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
