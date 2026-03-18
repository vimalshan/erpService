using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeTransfer : BaseEntity
{
    public long TransferId { get; private set; }
    public long EmployeeId { get; private set; }
    public string? OldUnit { get; private set; }
    public string? NewUnit { get; private set; }
    public long OldUnitId { get; private set; }
    public long NewUnitId { get; private set; }
    public long? ReasonId { get; private set; }
    public DateTime TransferDate { get; private set; }
    public string? Remarks { get; private set; }
    public string TransferType { get; private set; } = string.Empty;  // 01, 02, I, P, N
    public string Status { get; private set; } = "01";
    public bool PayrollTransfer { get; private set; }
    public bool TimeOfficeTransfer { get; private set; }
    public char ExpatStatus { get; private set; }
    public long? CreatedBy { get; private set; }
    public DateTime? CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeTransfer() { }

    public static EmployeeTransfer Create(long transferId, long employeeId, string? oldUnit,
        string? newUnit, long oldUnitId, long newUnitId, long? reasonId, DateTime transferDate,
        string? remarks, string transferType, bool payrollTransfer, long createdBy)
    {
        return new EmployeeTransfer
        {
            TransferId = transferId, EmployeeId = employeeId, OldUnit = oldUnit, NewUnit = newUnit,
            OldUnitId = oldUnitId, NewUnitId = newUnitId, ReasonId = reasonId,
            TransferDate = transferDate, Remarks = remarks, TransferType = transferType,
            Status = "01", PayrollTransfer = payrollTransfer, ExpatStatus = 'N',
            CreatedBy = createdBy, CreatedOn = DateTime.UtcNow
        };
    }

    public void Complete(long updatedBy)
    {
        Status = "04";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
