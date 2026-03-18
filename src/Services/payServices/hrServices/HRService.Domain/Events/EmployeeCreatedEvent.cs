namespace HRService.Domain.Events;

public class EmployeeCreatedEvent : Common.DomainEvent
{
    public Guid EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime JoinDate { get; set; }
}
