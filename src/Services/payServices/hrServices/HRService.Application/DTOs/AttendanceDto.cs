namespace HRService.Application.DTOs;

public class AttendanceDto
{
    public Guid AttendanceId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime AttendanceDate { get; set; }
    public Guid ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public string Status { get; set; }
    public string? Remarks { get; set; }
}

public class MarkAttendanceDto
{
    public Guid EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public Guid ShiftId { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public string Status { get; set; }
}
