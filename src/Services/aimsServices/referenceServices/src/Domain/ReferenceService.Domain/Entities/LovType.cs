namespace ReferenceService.Domain.Entities;

/// <summary>
/// Represents a List of Values Type (LOV_TYPEMAST).
/// Aggregate Root.
/// </summary>
public class LovType : Entity<int>
{
    public string TypeName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Sequence { get; private set; }
    public EntityStatus Status { get; private set; }
    
    private readonly List<LovValue> _values = [];
    public IReadOnlyCollection<LovValue> Values => _values.AsReadOnly();
    
    // For EF Core
    protected LovType() { }
    
    public static LovType Create(int id, string typeName, string? description, int sequence, long modifiedBy)
    {
        var lovType = new LovType
        {
            Id = id,
            TypeName = typeName,
            Description = description,
            Sequence = sequence,
            Status = EntityStatus.Active,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        
        lovType.AddDomainEvent(new LovTypeCreatedEvent(id, typeName));
        return lovType;
    }
    
    public void Update(string typeName, string? description, int sequence, long modifiedBy)
    {
        TypeName = typeName;
        Description = description;
        Sequence = sequence;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LovTypeUpdatedEvent(Id, typeName));
    }
    
    public void AddValue(LovValue value)
    {
        if (_values.Any(v => v.Code == value.Code))
            throw new InvalidOperationException($"LOV value with code '{value.Code}' already exists.");
        
        _values.Add(value);
    }
    
    public void Deactivate(long modifiedBy)
    {
        Status = EntityStatus.Inactive;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new LovTypeDeactivatedEvent(Id, TypeName));
    }
}

/// <summary>
/// Domain event for when a LOV Type is created.
/// </summary>
public record LovTypeCreatedEvent(int LovTypeId, string TypeName) : DomainEvent;

/// <summary>
/// Domain event for when a LOV Type is updated.
/// </summary>
public record LovTypeUpdatedEvent(int LovTypeId, string TypeName) : DomainEvent;

/// <summary>
/// Domain event for when a LOV Type is deactivated.
/// </summary>
public record LovTypeDeactivatedEvent(int LovTypeId, string TypeName) : DomainEvent;
