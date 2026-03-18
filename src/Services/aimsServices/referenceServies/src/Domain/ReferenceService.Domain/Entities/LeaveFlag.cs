namespace ReferenceService.Domain.Entities;

/// <summary>
/// Represents a Leave Flag (LEAVEFLAG).
/// Aggregate Root.
/// </summary>
public class LeaveFlag : Entity<int>
{
    public string Code { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? Type { get; private set; }
    public EntityStatus Status { get; private set; }
    
    // For EF Core
    protected LeaveFlag() { }
    
    public static LeaveFlag Create(int id, string code, string description, string? type, long modifiedBy)
    {
        var leaveFlag = new LeaveFlag
        {
            Id = id,
            Code = code,
            Description = description,
            Type = type,
            Status = EntityStatus.Active,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        
        leaveFlag.AddDomainEvent(new LeaveFlagCreatedEvent(id, code));
        return leaveFlag;
    }
    
    public void Update(string description, string? type, long modifiedBy)
    {
        Description = description;
        Type = type;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LeaveFlagUpdatedEvent(Id, Code));
    }
    
    public void Deactivate(long modifiedBy)
    {
        Status = EntityStatus.Inactive;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LeaveFlagDeactivatedEvent(Id, Code));
    }
}

/// <summary>
/// Domain event for when a Leave Flag is created.
/// </summary>
public record LeaveFlagCreatedEvent(int LeaveFlagId, string Code) : DomainEvent;

/// <summary>
/// Domain event for when a Leave Flag is updated.
/// </summary>
public record LeaveFlagUpdatedEvent(int LeaveFlagId, string Code) : DomainEvent;

/// <summary>
/// Domain event for when a Leave Flag is deactivated.
/// </summary>
public record LeaveFlagDeactivatedEvent(int LeaveFlagId, string Code) : DomainEvent;
