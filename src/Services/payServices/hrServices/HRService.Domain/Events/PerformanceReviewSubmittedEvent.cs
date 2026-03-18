namespace HRService.Domain.Events;

public class PerformanceReviewSubmittedEvent : Common.DomainEvent
{
    public Guid ReviewId { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal Rating { get; set; }
}
