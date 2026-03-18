namespace AuthProvider.Domain.Entities;

/// <summary>AuditLog entity – immutable audit trail record.</summary>
public class AuditLog
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string Resource { get; private set; } = string.Empty;
    public string? Details { get; private set; }
    public string IpAddress { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }
    public bool IsSuccess { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(
        Guid? userId,
        string action,
        string resource,
        string ipAddress,
        bool isSuccess,
        string? details = null) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            Resource = resource,
            IpAddress = ipAddress,
            IsSuccess = isSuccess,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
}
