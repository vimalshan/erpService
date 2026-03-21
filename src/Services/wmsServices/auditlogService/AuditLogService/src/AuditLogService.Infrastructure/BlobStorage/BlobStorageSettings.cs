namespace AuditLogService.Infrastructure.BlobStorage;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "audit-attachments";
}
