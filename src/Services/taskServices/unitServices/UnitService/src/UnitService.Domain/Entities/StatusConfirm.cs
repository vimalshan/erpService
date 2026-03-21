using UnitService.Domain.ValueObjects;

namespace UnitService.Domain.Entities;

public class StatusConfirm : BaseEntity
{
    public UnitCode UnitCode { get; private set; } = null!;
    public DateTime StatusDate { get; private set; }
    public decimal ConfirmedBy { get; private set; }
    public DateTime ConfirmedOn { get; private set; }

    private StatusConfirm() { }

    public static StatusConfirm Create(string unitCode, DateTime statusDate, decimal confirmedBy)
    {
        return new StatusConfirm
        {
            UnitCode = UnitCode.From(unitCode),
            StatusDate = statusDate,
            ConfirmedBy = confirmedBy,
            ConfirmedOn = DateTime.UtcNow
        };
    }
}
