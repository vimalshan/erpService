namespace AuditLogService.Application.DTOs;

public class AuditLogDto
{
    public long LogId { get; set; }
    public string TableName { get; set; } = null!;
    public int RecordId { get; set; }
    public string Action { get; set; } = null!;
    public string? ChangedBy { get; set; }
    public DateTime ChangeDate { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
}
