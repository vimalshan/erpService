namespace LovService.Domain.Entities;

public class LovType
{
    public long LovTypeId { get; private set; }
    public string LovTypeName { get; private set; } = string.Empty;

    private readonly List<LovMaster> _lovMasters = [];
    public IReadOnlyCollection<LovMaster> LovMasters => _lovMasters.AsReadOnly();

    private LovType() { }

    public static LovType Create(long lovTypeId, string lovTypeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lovTypeName);
        if (lovTypeName.Length > 30)
            throw new ArgumentException("LovTypeName cannot exceed 30 characters.", nameof(lovTypeName));

        return new LovType { LovTypeId = lovTypeId, LovTypeName = lovTypeName };
    }

    public void UpdateName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        if (newName.Length > 30)
            throw new ArgumentException("LovTypeName cannot exceed 30 characters.", nameof(newName));
        LovTypeName = newName;
    }
}
