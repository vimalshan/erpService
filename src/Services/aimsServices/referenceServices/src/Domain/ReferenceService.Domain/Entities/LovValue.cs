namespace ReferenceService.Domain.Entities;

/// <summary>
/// Represents a List of Values (LOV_MAST).
/// Child entity of LovType aggregate.
/// </summary>
public class LovValue : Entity<int>
{
    public int TypeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? LongDescription { get; private set; }
    public int Sequence { get; private set; }
    public EntityStatus Status { get; private set; }
    
    // For EF Core
    protected LovValue() { }
    
    public static LovValue Create(int id, int typeId, string code, string description, 
        string? longDescription, int sequence, long modifiedBy)
    {
        var lovValue = new LovValue
        {
            Id = id,
            TypeId = typeId,
            Code = code,
            Description = description,
            LongDescription = longDescription,
            Sequence = sequence,
            Status = EntityStatus.Active,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        
        lovValue.AddDomainEvent(new LovValueCreatedEvent(id, typeId, code));
        return lovValue;
    }
    
    public void Update(string description, string? longDescription, int sequence, long modifiedBy)
    {
        Description = description;
        LongDescription = longDescription;
        Sequence = sequence;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LovValueUpdatedEvent(Id, TypeId, Code));
    }
    
    public void Deactivate(long modifiedBy)
    {
        Status = EntityStatus.Inactive;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LovValueDeactivatedEvent(Id, TypeId, Code));
    }
}

/// <summary>
/// Domain event for when a LOV Value is created.
/// </summary>
public record LovValueCreatedEvent(int LovValueId, int LovTypeId, string Code) : DomainEvent;

/// <summary>
/// Domain event for when a LOV Value is updated.
/// </summary>
public record LovValueUpdatedEvent(int LovValueId, int LovTypeId, string Code) : DomainEvent;

/// <summary>
/// Domain event for when a LOV Value is deactivated.
/// </summary>
public record LovValueDeactivatedEvent(int LovValueId, int LovTypeId, string Code) : DomainEvent;
