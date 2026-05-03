namespace DocumentService.Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public Guid DocumentId { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? StoragePath { get; set; }
    public string? Category { get; set; }
    public int? AuditId { get; set; }
    public int? FindingId { get; set; }
    public int? CertificateId { get; set; }
    public int? ContractId { get; set; }
    public string? UploadedBy { get; set; }
    public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}
