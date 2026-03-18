using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class FlWorkingShift : Entity
{
    public decimal? FlWorkingId { get; private set; }
    public decimal FillingLineId { get; private set; }
    public char ShiftCode { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? CloseDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    protected FlWorkingShift() { }

    public static FlWorkingShift Create(decimal lineId, char shiftCode, DateTime startDate)
    {
        return new FlWorkingShift
        {
            FillingLineId = lineId,
            ShiftCode = shiftCode,
            StartDate = startDate
        };
    }

    public void Close(int modifiedBy)
    {
        CloseDate = DateTime.UtcNow;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
