using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class MedicinePackaging : AuditableEntity, IAggregateRoot
{
    public string PackagingCode { get; private set; } = null!;
    public string? PackagingType { get; private set; }

    private MedicinePackaging() { }

    public static MedicinePackaging Create(string packagingCode, string? packagingType, string entryUser, decimal? userPin)
    {
        return new MedicinePackaging
        {
            PackagingCode = packagingCode,
            PackagingType = packagingType,
            EntryUser = entryUser,
            EntryUserPin = userPin,
            EntryDate = DateTime.UtcNow
        };
    }

    public void Update(string? packagingType, string modifiedUser, decimal? modifiedUserPin)
    {
        PackagingType = packagingType;
        ModifiedUser = modifiedUser;
        ModifiedUserPin = modifiedUserPin;
        ModifiedDate = DateTime.UtcNow;
    }
}
