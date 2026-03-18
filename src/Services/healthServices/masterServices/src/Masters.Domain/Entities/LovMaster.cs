using Masters.Domain.Common;
using Masters.Domain.ValueObjects;

namespace Masters.Domain.Entities;

public class LovMaster : BaseEntity
{
    public LovTypeCode LovType { get; private set; } = null!;
    public long LovId { get; private set; }
    public string LovName { get; private set; } = string.Empty;

    // Navigation property
    public LovTypeMaster? LovTypeMaster { get; private set; }

    // EF Core constructor
    private LovMaster() { }

    public LovMaster(LovTypeCode lovType, long lovId, string lovName)
    {
        LovType = lovType ?? throw new ArgumentNullException(nameof(lovType));
        LovId = lovId;
        SetLovName(lovName);
    }

    public void SetLovName(string lovName)
    {
        if (string.IsNullOrWhiteSpace(lovName))
            throw new ArgumentException("LOV Name cannot be empty.", nameof(lovName));
        
        if (lovName.Length > 2000)
            throw new ArgumentException("LOV Name cannot exceed 2000 characters.", nameof(lovName));

        LovName = lovName;
    }

    public void ChangeLovType(LovTypeCode newLovType)
    {
        if (newLovType == null)
            throw new ArgumentNullException(nameof(newLovType));

        LovType = newLovType;
    }
}
