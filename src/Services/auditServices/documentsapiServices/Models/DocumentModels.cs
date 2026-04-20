namespace DocumentService.Models;

public class DocumentDto
{
    public string DocumentId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedDate { get; set; }
}

public class DocumentUploadRequest
{
    public int? AuditId { get; set; }
    public int? FindingId { get; set; }
    public int? CertificateId { get; set; }
    public int? ContractId { get; set; }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public T? Data { get; set; }
}
