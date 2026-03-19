namespace OrderScheduleService.Application.DTOs;

public class CreateShiftDto
{
    public char ShiftCode { get; set; }
    public string ShiftDescription { get; set; } = null!;
    public decimal CompanyUnitId { get; set; }
    public string StartTime { get; set; } = null!;
    public int StartDay { get; set; }
    public string EndTime { get; set; } = null!;
    public int EndDay { get; set; }
}

public class ShiftDto
{
    public char ShiftCode { get; set; }
    public string ShiftDescription { get; set; } = null!;
    public decimal CompanyUnitId { get; set; }
    public string StartTime { get; set; } = null!;
    public int StartDay { get; set; }
    public string EndTime { get; set; } = null!;
    public int EndDay { get; set; }
}
