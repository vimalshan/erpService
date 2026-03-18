namespace HRService.Domain.Entities;

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    EarlyLeave
}

public class Attendance : Common.AggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime AttendanceDate { get; private set; }
    public Guid ShiftId { get; private set; }
    public TimeSpan? CheckInTime { get; private set; }
    public TimeSpan? CheckOutTime { get; private set; }
    public AttendanceStatus Status { get; private set; }
    public string? Remarks { get; private set; }

    private Attendance() { }

    public static Attendance Create(
        Guid employeeId,
        DateTime attendanceDate,
        Guid shiftId,
        TimeSpan? checkInTime = null,
        TimeSpan? checkOutTime = null,
        AttendanceStatus status = AttendanceStatus.Absent)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee id cannot be empty", nameof(employeeId));

        if (shiftId == Guid.Empty)
            throw new ArgumentException("Shift id cannot be empty", nameof(shiftId));

        if (attendanceDate > DateTime.Today)
            throw new ArgumentException("Attendance date cannot be in the future");

        return new Attendance
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            AttendanceDate = attendanceDate,
            ShiftId = shiftId,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            Status = status,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void MarkPresent(TimeSpan? checkInTime = null, TimeSpan? checkOutTime = null)
    {
        CheckInTime = checkInTime;
        CheckOutTime = checkOutTime;
        Status = AttendanceStatus.Present;
        ModifiedDate = DateTime.UtcNow;
    }

    public void MarkAbsent()
    {
        Status = AttendanceStatus.Absent;
        CheckInTime = null;
        CheckOutTime = null;
        ModifiedDate = DateTime.UtcNow;
    }

    public void MarkLate(TimeSpan checkInTime)
    {
        CheckInTime = checkInTime;
        Status = AttendanceStatus.Late;
        ModifiedDate = DateTime.UtcNow;
    }

    public void MarkEarlyLeave(TimeSpan checkOutTime)
    {
        CheckOutTime = checkOutTime;
        Status = AttendanceStatus.EarlyLeave;
        ModifiedDate = DateTime.UtcNow;
    }

    public void AddRemarks(string remarks)
    {
        Remarks = remarks;
        ModifiedDate = DateTime.UtcNow;
    }
}
