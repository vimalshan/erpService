namespace CertificateService.Application.DTOs;

public record CertificateDto(int CertificateId, string CertificateNumber, string CertificateName, int CompanyId,
    int? SiteId, int ServiceId, DateTime IssueDate, DateTime ExpiryDate, string Status, string? CertificateType, string? Scope, bool IsActive);

public record CreateCertificateDto(string CertificateNumber, string CertificateName, int CompanyId,
    int? SiteId, int ServiceId, DateTime IssueDate, DateTime ExpiryDate, string? CertificateType, string? Scope);

public record UpdateCertificateDto(int CertificateId, string CertificateNumber, string CertificateName, int CompanyId,
    int? SiteId, int ServiceId, DateTime IssueDate, DateTime ExpiryDate, string Status, string? CertificateType, string? Scope);
