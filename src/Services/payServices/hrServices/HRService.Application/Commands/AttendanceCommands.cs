using MediatR;

namespace HRService.Application.Commands;

public class MarkAttendanceCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public Guid ShiftId { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public string Status { get; set; }
}

public class UpdateAttendanceCommand : IRequest<bool>
{
    public Guid AttendanceId { get; set; }
    public TimeSpan? CheckInTime { get; set; }
    public TimeSpan? CheckOutTime { get; set; }
    public string Status { get; set; }
}
