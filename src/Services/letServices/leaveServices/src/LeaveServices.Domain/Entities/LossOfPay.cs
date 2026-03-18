using LeaveServices.Domain.Common;
using LeaveServices.Domain.Events;

namespace LeaveServices.Domain.Entities;

/// <summary>
/// Aggregate Root: Loss of Pay (maps to LOSS_OF_PAY)
/// </summary>
public sealed class LossOfPay : BaseEntity
{
    public long LopId { get; private set; }
    public long EmpSysId { get; private set; }
    public int LopDays { get; private set; }
    public DateOnly LopMonth { get; private set; }
    public string? LopRemarks { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public long? ModifiedBy { get; private set; }

    private LossOfPay() { }

    public static LossOfPay Record(
        long empSysId,
        int lopDays,
        DateOnly lopMonth,
        string? remarks,
        long recordedBy)
    {
        if (lopDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(lopDays), "LOP days must be positive.");

        var entity = new LossOfPay
        {
            EmpSysId = empSysId,
            LopDays = lopDays,
            LopMonth = lopMonth,
            LopRemarks = remarks,
            CreatedBy = recordedBy,
            CreatedOn = DateTime.UtcNow
        };

        entity.RaiseDomainEvent(new LossOfPayRecordedEvent(0, empSysId, lopDays, lopMonth));
        return entity;
    }
}
