namespace ReferenceService.Domain.Entities;

/// <summary>
/// Represents a Permission Rule (PERMISSION_RULES).
/// Aggregate Root.
/// </summary>
public class PermissionRule : Entity<int>
{
    public string ResourceId { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string AppCode { get; private set; } = string.Empty;
    public EntityStatus Status { get; private set; }
    
    // For EF Core
    protected PermissionRule() { }
    
    public static PermissionRule Create(int id, string resourceId, string action, 
        string? description, string appCode, long modifiedBy)
    {
        var rule = new PermissionRule
        {
            Id = id,
            ResourceId = resourceId,
            Action = action,
            Description = description,
            AppCode = appCode,
            Status = EntityStatus.Active,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        
        rule.AddDomainEvent(new PermissionRuleCreatedEvent(id, resourceId, action));
        return rule;
    }
    
    public void Update(string resourceId, string action, string? description, string appCode, long modifiedBy)
    {
        ResourceId = resourceId;
        Action = action;
        Description = description;
        AppCode = appCode;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new PermissionRuleUpdatedEvent(Id, resourceId, action));
    }
}

/// <summary>
/// Domain event for when a Permission Rule is created.
/// </summary>
public record PermissionRuleCreatedEvent(int PermissionRuleId, string ResourceId, string Action) : DomainEvent;

/// <summary>
/// Domain event for when a Permission Rule is updated.
/// </summary>
public record PermissionRuleUpdatedEvent(int PermissionRuleId, string ResourceId, string Action) : DomainEvent;
