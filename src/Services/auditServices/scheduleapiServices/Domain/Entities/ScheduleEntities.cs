using ScheduleService.Domain.Events;

namespace ScheduleService.Domain.Entities;

public class AuditSiteAudit
{
    public int AuditSiteAuditId { get; set; }
    public int AuditId { get; set; }
    public int SiteId { get; set; }
    public int AuditTypeId { get; set; }
    public string AuditNumber { get; set; } = string.Empty;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Status { get; set; } = "scheduled";
    public int? LeadAuditorId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Notes { get; set; }
    public string? ReportPath { get; set; }
    public bool CertificateIssued { get; set; }
    public string? CertificateNumber { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public static AuditSiteAudit Schedule(int auditId, int siteId, int auditTypeId, string auditNumber,
        DateTime? scheduledDate, int? leadAuditorId, int? createdBy)
    {
        var s = new AuditSiteAudit
        {
            AuditId = auditId, SiteId = siteId, AuditTypeId = auditTypeId, AuditNumber = auditNumber,
            ScheduledDate = scheduledDate, LeadAuditorId = leadAuditorId,
            Status = "scheduled", IsActive = true, CreatedBy = createdBy, ModifiedBy = createdBy,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        s._domainEvents.Add(new AuditScheduledEvent(0, auditNumber, auditId, siteId, scheduledDate));
        return s;
    }

    public void Reschedule(DateTime? newDate, int? modifiedBy)
    {
        var oldDate = ScheduledDate;
        ScheduledDate = newDate; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new AuditRescheduledEvent(AuditSiteAuditId, AuditNumber, oldDate, newDate));
    }

    public void Start(DateTime startDate, int? modifiedBy)
    {
        StartDate = startDate; Status = "in-progress"; ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new AuditStartedEvent(AuditSiteAuditId, AuditNumber, startDate));
    }

    public void Complete(DateTime completedDate, string? reportPath, int? modifiedBy)
    {
        CompletedDate = completedDate; Status = "completed"; ReportPath = reportPath;
        ModifiedDate = DateTime.UtcNow; ModifiedBy = modifiedBy;
        _domainEvents.Add(new AuditCompletedEvent(AuditSiteAuditId, AuditNumber, completedDate));
    }
}
