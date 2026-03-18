namespace HRService.Domain.Events;

public class EmployeeTerminatedEvent : Common.DomainEvent
{
    public Guid EmployeeId { get; set; }
    public DateTime TerminationDate { get; set; }
    public string Reason { get; set; }
}
