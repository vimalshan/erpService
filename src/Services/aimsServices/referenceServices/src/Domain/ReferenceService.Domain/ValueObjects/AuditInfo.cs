namespace ReferenceService.Domain.ValueObjects;

/// <summary>
/// Represents an audit information value object.
/// </summary>
public record AuditInfo
{
    public long ModifiedBy { get; init; }
    public DateTime ModifiedOn { get; init; }
    
    public static AuditInfo Create(long modifiedBy) => new()
    {
        ModifiedBy = modifiedBy,
        ModifiedOn = DateTime.UtcNow
    };
}
