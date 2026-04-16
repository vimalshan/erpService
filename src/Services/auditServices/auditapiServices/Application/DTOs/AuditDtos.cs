namespace AuditService.Application.DTOs;

public record AuditDto(
    int AuditId, string? Sites, string? Services, int? CompanyId,
    string? Status, DateTime? StartDate, DateTime? EndDate,
    string? LeadAuditor, string? Type);

public record AuditTypeDto(
    int AuditTypeId, string AuditTypeName, string AuditTypeCode,
    string? Description, int? Duration, bool IsActive, string? Category);

public record AuditSiteAuditDto(
    int AuditSiteAuditId, int AuditId, int SiteId, int AuditTypeId,
    string AuditNumber, DateTime? ScheduledDate, DateTime? StartDate,
    DateTime? EndDate, string Status, int? LeadAuditorId);

public record CreateAuditDto(
    string? Sites, string? Services, int? CompanyId,
    string? Status, DateTime? StartDate, DateTime? EndDate,
    string? LeadAuditor, string? Type);

public record UpdateAuditDto(
    int AuditId, string? Sites, string? Services, int? CompanyId,
    string? Status, DateTime? StartDate, DateTime? EndDate,
    string? LeadAuditor, string? Type);
