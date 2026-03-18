using Masters.Domain.Common;
using Masters.Domain.ValueObjects;

namespace Masters.Domain.Entities;

public class LovTypeMaster : BaseEntity, IAggregateRoot
{
    private readonly List<LovMaster> _lovValues = new();

    public LovTypeCode LovTypeCode { get; private set; } = null!;
    public string LovTypeName { get; private set; } = string.Empty;
    public IReadOnlyCollection<LovMaster> LovValues => _lovValues.AsReadOnly();

    // EF Core constructor
    private LovTypeMaster() { }

    public LovTypeMaster(LovTypeCode lovTypeCode, string lovTypeName)
    {
        LovTypeCode = lovTypeCode ?? throw new ArgumentNullException(nameof(lovTypeCode));
        SetLovTypeName(lovTypeName);
    }

    public void SetLovTypeName(string lovTypeName)
    {
        if (string.IsNullOrWhiteSpace(lovTypeName))
            throw new ArgumentException("LOV Type Name cannot be empty.", nameof(lovTypeName));
        
        if (lovTypeName.Length > 50)
            throw new ArgumentException("LOV Type Name cannot exceed 50 characters.", nameof(lovTypeName));

        LovTypeName = lovTypeName;
    }

    public void AddLovValue(LovMaster lovValue)
    {
        if (lovValue == null)
            throw new ArgumentNullException(nameof(lovValue));

        if (!lovValue.LovType.Equals(LovTypeCode))
            throw new InvalidOperationException($"LOV value type '{lovValue.LovType}' does not match '{LovTypeCode}'.");

        _lovValues.Add(lovValue);
    }

    public void RemoveLovValue(long lovId)
    {
        var lovValue = _lovValues.FirstOrDefault(x => x.LovId == lovId);
        if (lovValue != null)
        {
            _lovValues.Remove(lovValue);
        }
    }
}
