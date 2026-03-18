namespace HRService.Domain.Events;

public class LeaveApprovedEvent : Common.DomainEvent
{
    public Guid LeaveId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
