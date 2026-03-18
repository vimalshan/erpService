using MediatR;
using Shared.Core.Domain;

namespace Shared.Events.Domain;

#region Accident Events
/// <summary>
/// Published when an accident is created
/// </summary>
public class AccidentCreatedEvent : DomainEvent, INotification
{
    public string AccidentId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string SiteCode { get; set; } = string.Empty;
    public string AccidentType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime AccidentDateTime { get; set; }
}

/// <summary>
/// Published when an accident status is updated
/// </summary>
public class AccidentStatusUpdatedEvent : DomainEvent, INotification
{
    public string AccidentId { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
}
#endregion

#region Checkup Events
/// <summary>
/// Published when a checkup is scheduled
/// </summary>
public class CheckupScheduledEvent : DomainEvent, INotification
{
    public string CheckupId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CheckupType { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string Department { get; set; } = string.Empty;
}

/// <summary>
/// Published when a checkup is conducted
/// </summary>
public class CheckupConductedEvent : DomainEvent, INotification
{
    public string CheckupId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string ConductingDoctor { get; set; } = string.Empty;
    public DateTime ConductedDate { get; set; }
    public bool FollowUpRequired { get; set; }
}
#endregion

#region Medicine Events
/// <summary>
/// Published when medicine is issued to an employee
/// </summary>
public class MedicineIssuedEvent : DomainEvent, INotification
{
    public string IssueId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string MedicineId { get; set; } = string.Empty;
    public int QuantityIssued { get; set; }
    public DateTime IssueDate { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
}

/// <summary>
/// Published when medicine purchase is completed
/// </summary>
public class MedicinePurchasedEvent : DomainEvent, INotification
{
    public string PurchaseId { get; set; } = string.Empty;
    public string MedicineId { get; set; } = string.Empty;
    public int QuantityReceived { get; set; }
    public DateTime PurchaseDate { get; set; }
}

/// <summary>
/// Published when medicine expires
/// </summary>
public class MedicineExpiredEvent : DomainEvent, INotification
{
    public string MedicineId { get; set; } = string.Empty;
    public string MedicineName { get; set; } = string.Empty;
    public int QuantityExpired { get; set; }
    public DateTime ExpiryDate { get; set; }
}
#endregion

#region Visit Events
/// <summary>
/// Published when an employee checks in to a site
/// </summary>
public class VisitRecordedEvent : DomainEvent, INotification
{
    public string VisitId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string SiteCode { get; set; } = string.Empty;
    public string VisitPurpose { get; set; } = string.Empty;
    public DateTime CheckInTime { get; set; }
}

/// <summary>
/// Published when an employee checks out from a site
/// </summary>
public class VisitCompletedEvent : DomainEvent, INotification
{
    public string VisitId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime CheckOutTime { get; set; }
    public decimal DurationHours { get; set; }
}

/// <summary>
/// Published when a visit is approved
/// </summary>
public class VisitApprovedEvent : DomainEvent, INotification
{
    public string VisitId { get; set; } = string.Empty;
    public string ApprovalStatus { get; set; } = string.Empty;
    public string ApprovedBy { get; set; } = string.Empty;
}

/// <summary>
/// Published when a violation is recorded for a visit
/// </summary>
public class VisitViolationRecordedEvent : DomainEvent, INotification
{
    public string ViolationId { get; set; } = string.Empty;
    public string VisitId { get; set; } = string.Empty;
    public string ViolationType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public DateTime RecordedOn { get; set; }
}
#endregion

#region Lookup Events
/// <summary>
/// Published when a new lookup value is created
/// </summary>
public class LookupCreatedEvent : DomainEvent, INotification
{
    public string LookupId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Published when a lookup is updated
/// </summary>
public class LookupUpdatedEvent : DomainEvent, INotification
{
    public string LookupId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Published when a lookup is deleted
/// </summary>
public class LookupDeletedEvent : DomainEvent, INotification
{
    public string LookupId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
#endregion
