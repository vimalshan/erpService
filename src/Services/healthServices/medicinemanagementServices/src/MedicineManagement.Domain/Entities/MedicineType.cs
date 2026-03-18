using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class MedicineType : AuditableEntity, IAggregateRoot
{
    public string TypeCode { get; private set; } = null!;
    public string? TypeName { get; private set; }

    private MedicineType() { }

    public static MedicineType Create(string typeCode, string? typeName, string entryUser, decimal? userPin)
    {
        var entity = new MedicineType
        {
            TypeCode = typeCode,
            TypeName = typeName,
            EntryUser = entryUser,
            EntryUserPin = userPin,
            EntryDate = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.MedicineTypeCreatedEvent(entity));
        return entity;
    }

    public void Update(string? typeName, string modifiedUser, decimal? modifiedUserPin)
    {
        TypeName = typeName;
        ModifiedUser = modifiedUser;
        ModifiedUserPin = modifiedUserPin;
        ModifiedDate = DateTime.UtcNow;
    }
}
