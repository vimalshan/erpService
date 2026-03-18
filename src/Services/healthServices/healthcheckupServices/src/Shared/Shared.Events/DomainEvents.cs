namespace Shared.Events;

using System.Text.Json.Serialization;

/// <summary>
/// Base event class for domain events and integration events
/// </summary>
public abstract class DomainEvent
{
    [JsonPropertyName("eventId")]
    public string EventId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = string.Empty;

    [JsonPropertyName("occurredOn")]
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;

    public DomainEvent()
    {
        EventType = GetType().Name;
    }
}

/// <summary>
/// Integration event for cross-service communication
/// </summary>
public abstract class IntegrationEvent : DomainEvent
{
}

/// <summary>
/// Common domain events
/// </summary>

/// <summary>
/// Event when an accident/FIR is created
/// </summary>
public class AccidentCreatedEvent : IntegrationEvent
{
    public string AccidentNumber { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime AccidentDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int InjuredPersonCount { get; set; }

    public AccidentCreatedEvent(string accidentNumber, string employeeNumber, DateTime accidentDate, string location, string description)
    {
        AccidentNumber = accidentNumber;
        EmployeeNumber = employeeNumber;
        AccidentDate = accidentDate;
        Location = location;
        Description = description;
        Source = "AccidentManagementService";
    }
}

/// <summary>
/// Event when an accident is updated
/// </summary>
public class AccidentUpdatedEvent : IntegrationEvent
{
    public string AccidentNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    public AccidentUpdatedEvent(string accidentNumber, string status, string updatedBy)
    {
        AccidentNumber = accidentNumber;
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Source = "AccidentManagementService";
    }
}

/// <summary>
/// Event when a checkup is scheduled
/// </summary>
public class CheckupScheduledEvent : IntegrationEvent
{
    public string CheckupId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string CheckupType { get; set; } = string.Empty;

    public CheckupScheduledEvent(string checkupId, string employeeNumber, DateTime scheduledDate, string checkupType)
    {
        CheckupId = checkupId;
        EmployeeNumber = employeeNumber;
        ScheduledDate = scheduledDate;
        CheckupType = checkupType;
        Source = "CheckupManagementService";
    }
}

/// <summary>
/// Event when a medicine is issued
/// </summary>
public class MedicineIssuedEvent : IntegrationEvent
{
    public string IssueNumber { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string MedicineId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime IssuedDate { get; set; }

    public MedicineIssuedEvent(string issueNumber, string employeeNumber, string medicineId, int quantity)
    {
        IssueNumber = issueNumber;
        EmployeeNumber = employeeNumber;
        MedicineId = medicineId;
        Quantity = quantity;
        IssuedDate = DateTime.UtcNow;
        Source = "MedicineManagementService";
    }
}

/// <summary>
/// Event when a visit is recorded
/// </summary>
public class VisitRecordedEvent : IntegrationEvent
{
    public string VisitId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string DoctorCode { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Diagnosis { get; set; } = string.Empty;

    public VisitRecordedEvent(string visitId, string employeeNumber, string doctorCode, string diagnosis)
    {
        VisitId = visitId;
        EmployeeNumber = employeeNumber;
        DoctorCode = doctorCode;
        VisitDate = DateTime.UtcNow;
        Diagnosis = diagnosis;
        Source = "VisitManagementService";
    }
}

/// <summary>
/// Event when lookup data is created
/// </summary>
public class LookupCreatedEvent : IntegrationEvent
{
    public string LookupId { get; set; } = string.Empty;
    public string LookupType { get; set; } = string.Empty;
    public string LookupValue { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public LookupCreatedEvent(string lookupId, string lookupType, string lookupValue, string description)
    {
        LookupId = lookupId;
        LookupType = lookupType;
        LookupValue = lookupValue;
        Description = description;
        Source = "LookupManagementService";
    }
}

/// <summary>
/// Interface for event publishers
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : DomainEvent;
    Task PublishBatchAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : DomainEvent;
}

/// <summary>
/// Interface for event handlers
/// </summary>
public interface IEventHandler<T> where T : DomainEvent
{
    Task HandleAsync(T @event, CancellationToken cancellationToken = default);
}

/// <summary>
/// Event subscription for observer pattern
/// </summary>
public interface IEventSubscriber
{
    void Subscribe<T>(IEventHandler<T> handler) where T : DomainEvent;
    void Unsubscribe<T>(IEventHandler<T> handler) where T : DomainEvent;
}
