using CertificateService.Domain.Events;

namespace CertificateService.Domain.Entities;

public class Certificate
{
    public int CertificateId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string CertificateName { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int? SiteId { get; set; }
    public int ServiceId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Status { get; set; } = "Active";
    public string? CertificateType { get; set; }
    public string? Scope { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public int? IssuedBy { get; set; }
    public int RevisionNumber { get; set; } = 1;
    public int? PreviousCertificateId { get; set; }
    public string? CertificatePath { get; set; }
    public int? AuditId { get; set; }
    public string? Notes { get; set; }

    public ICollection<CertificateServiceEntity> CertificateServices { get; set; } = new List<CertificateServiceEntity>();
    public ICollection<CertificateSite> CertificateSites { get; set; } = new List<CertificateSite>();
    public ICollection<CertificateAdditionalScope> AdditionalScopes { get; set; } = new List<CertificateAdditionalScope>();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent e) => _domainEvents.Add(e);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

public class CertificateServiceEntity
{
    public int CertificateServiceId { get; set; }
    public int CertificateId { get; set; }
    public int ServiceId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Scope { get; set; }
    public string? Notes { get; set; }
    public Certificate? Certificate { get; set; }
}

public class CertificateSite
{
    public int CertificateSiteId { get; set; }
    public int CertificateId { get; set; }
    public int SiteId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Scope { get; set; }
    public string? Notes { get; set; }
    public Certificate? Certificate { get; set; }
}

public class CertificateAdditionalScope
{
    public int CertificateAdditionalScopeId { get; set; }
    public int CertificateId { get; set; }
    public string ScopeDescription { get; set; } = string.Empty;
    public string? ScopeType { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Notes { get; set; }
    public Certificate? Certificate { get; set; }
}
