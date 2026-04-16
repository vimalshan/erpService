using AuditService.Domain.Events;

namespace AuditService.Domain.Entities;

public class Audit
{
    public int AuditId { get; set; }
    public string? Sites { get; set; }
    public string? Services { get; set; }
    public int? CompanyId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? LeadAuditor { get; set; }
    public string? Type { get; set; }

    public ICollection<AuditSite> AuditSites { get; set; } = new List<AuditSite>();
    public ICollection<AuditServiceEntity> AuditServices { get; set; } = new List<AuditServiceEntity>();
    public ICollection<AuditTeamMember> AuditTeamMembers { get; set; } = new List<AuditTeamMember>();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public void ChangeStatus(string newStatus)
    {
        var oldStatus = Status ?? "Unknown";
        Status = newStatus;
        AddDomainEvent(new AuditStatusChangedEvent(AuditId, oldStatus, newStatus));
    }
}

public class AuditType
{
    public int AuditTypeId { get; set; }
    public string AuditTypeName { get; set; } = string.Empty;
    public string AuditTypeCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Category { get; set; }
    public string? RequiredCertifications { get; set; }
    public int? DisplayOrder { get; set; }
}

public class AuditSite
{
    public int AuditSiteId { get; set; }
    public int AuditId { get; set; }
    public int SiteId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Status { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }
    public Audit? Audit { get; set; }
}

public class AuditServiceEntity
{
    public int AuditServiceId { get; set; }
    public int AuditId { get; set; }
    public int ServiceId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    public Audit? Audit { get; set; }
}

public class AuditTeamMember
{
    public int AuditTeamMemberId { get; set; }
    public int AuditId { get; set; }
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? AssignedDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Specialization { get; set; }
    public string? Notes { get; set; }
    public Audit? Audit { get; set; }
}

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
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Notes { get; set; }
    public string? ReportPath { get; set; }
    public bool CertificateIssued { get; set; }
    public string? CertificateNumber { get; set; }
    public AuditType? AuditTypeNavigation { get; set; }
    public ICollection<AuditSiteRepresentative> Representatives { get; set; } = new List<AuditSiteRepresentative>();
    public ICollection<AuditSiteService> SiteServices { get; set; } = new List<AuditSiteService>();
}

public class AuditSiteRepresentative
{
    public int AuditSiteRepresentativeId { get; set; }
    public int AuditSiteAuditId { get; set; }
    public int UserId { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? Notes { get; set; }
    public AuditSiteAudit? AuditSiteAudit { get; set; }
}

public class AuditSiteService
{
    public int AuditSiteServiceId { get; set; }
    public int AuditSiteAuditId { get; set; }
    public int ServiceId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public string? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    public decimal? Cost { get; set; }
    public string? Currency { get; set; }
    public AuditSiteAudit? AuditSiteAudit { get; set; }
}

/// <summary>Read-only projection of the Sites table used by AuditRepository.</summary>
public class SiteInfo
{
    public int SiteId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string? Location { get; set; }
}
