namespace OrderScheduleService.Domain.Entities;

using OrderScheduleService.Domain.Common;

public class Shift : Entity
{
    public char ShiftCode { get; set; }
    public string ShiftDescription { get; set; } = null!;
    public decimal CompanyUnitId { get; set; }
    public string StartTime { get; set; } = null!;
    public int StartDay { get; set; }
    public string EndTime { get; set; } = null!;
    public int EndDay { get; set; }

    public Shift() { }

    public Shift(
        char shiftCode,
        string shiftDescription,
        decimal companyUnitId,
        string startTime,
        int startDay,
        string endTime,
        int endDay)
    {
        ShiftCode = shiftCode;
        ShiftDescription = shiftDescription;
        CompanyUnitId = companyUnitId;
        StartTime = startTime;
        StartDay = startDay;
        EndTime = endTime;
        EndDay = endDay;
    }

    public bool IsNightShift() => EndDay > StartDay || TimeSpan.Parse(EndTime) < TimeSpan.Parse(StartTime);
}
