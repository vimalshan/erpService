namespace ScheduleService.Application.DTOs;

public record AuditSiteAuditDto(
    int AuditSiteAuditId, int AuditId, int SiteId, int AuditTypeId, string AuditNumber,
    DateTime? ScheduledDate, DateTime? StartDate, DateTime? EndDate, DateTime? CompletedDate,
    string Status, int? LeadAuditorId, bool IsActive, DateTime CreatedDate, DateTime ModifiedDate,
    int? CreatedBy, int? ModifiedBy, string? Notes, string? ReportPath,
    bool CertificateIssued, string? CertificateNumber);

public record CreateAuditSiteAuditDto(
    int AuditId, int SiteId, int AuditTypeId, string AuditNumber, DateTime? ScheduledDate,
    int? LeadAuditorId, string? Notes, int? CreatedBy);

public record UpdateAuditSiteAuditDto(
    int AuditSiteAuditId, int AuditId, int SiteId, int AuditTypeId, string AuditNumber,
    DateTime? ScheduledDate, DateTime? StartDate, DateTime? EndDate, string Status,
    int? LeadAuditorId, bool IsActive, string? Notes, string? ReportPath,
    bool CertificateIssued, string? CertificateNumber, int? ModifiedBy);
