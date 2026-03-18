namespace HRService.Domain.Events;

public class SalaryUpdatedEvent : Common.DomainEvent
{
    public Guid EmployeeId { get; set; }
    public Guid SalaryId { get; set; }
    public decimal NewSalary { get; set; }
    public DateTime EffectiveDate { get; set; }
}
