using Recruitment.Domain.Common;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Events;

public class ApplicationCreatedEvent : DomainEvent
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public string SparshId { get; set; }
    public DateTime CreatedDate { get; set; }

    public ApplicationCreatedEvent(decimal applicationNumber, decimal jobId, string sparshId)
    {
        ApplicationNumber = applicationNumber;
        JobId = jobId;
        SparshId = sparshId;
        CreatedDate = DateTime.UtcNow;
    }
}

public class ApplicationStatusChangedEvent : DomainEvent
{
    public decimal ApplicationNumber { get; set; }
    public ApplicationStatus NewStatus { get; set; }
    public string Remark { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime StatusChangeDate { get; set; }

    public ApplicationStatusChangedEvent(decimal applicationNumber, ApplicationStatus newStatus, string remark, string updatedBy)
    {
        ApplicationNumber = applicationNumber;
        NewStatus = newStatus;
        Remark = remark;
        UpdatedBy = updatedBy;
        StatusChangeDate = DateTime.UtcNow;
    }
}

public class ApplicationShortlistedEvent : DomainEvent
{
    public decimal ApplicationNumber { get; set; }
    public DateTime ShortlistedDate { get; set; }

    public ApplicationShortlistedEvent(decimal applicationNumber)
    {
        ApplicationNumber = applicationNumber;
        ShortlistedDate = DateTime.UtcNow;
    }
}

public class ApplicationSelectedEvent : DomainEvent
{
    public decimal ApplicationNumber { get; set; }
    public decimal JobId { get; set; }
    public DateTime SelectedDate { get; set; }

    public ApplicationSelectedEvent(decimal applicationNumber, decimal jobId)
    {
        ApplicationNumber = applicationNumber;
        JobId = jobId;
        SelectedDate = DateTime.UtcNow;
    }
}
