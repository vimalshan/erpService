namespace HRService.Application.DTOs;

public class LeaveDto
{
    public Guid LeaveId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public Guid LeaveTypeId { get; set; }
    public string? LeaveTypeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfDays { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class RequestLeaveDto
{
    public Guid EmployeeId { get; set; }
    public Guid LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class ApproveLeaveDto
{
    public Guid LeaveId { get; set; }
    public Guid ApprovedBy { get; set; }
}
