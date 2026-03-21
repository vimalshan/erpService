using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class LovTypeMaster : BaseEntity
{
    public string LovTypeCode { get; private set; } = null!;
    public string? LovTypeName { get; private set; }

    // Navigation
    public ICollection<LovMaster> LovMasters { get; private set; } = [];

    private LovTypeMaster() { }

    public static LovTypeMaster Create(string typeCode, string? typeName)
    {
        if (string.IsNullOrWhiteSpace(typeCode) || typeCode.Length > 3)
            throw new ArgumentException("Type code must be 1-3 characters.");

        return new LovTypeMaster
        {
            LovTypeCode = typeCode.PadRight(3),
            LovTypeName = typeName
        };
    }

    public void UpdateName(string? name)
    {
        LovTypeName = name;
    }
}
