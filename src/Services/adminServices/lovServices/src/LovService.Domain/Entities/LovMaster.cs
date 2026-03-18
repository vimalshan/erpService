namespace LovService.Domain.Entities;

public class LovMaster
{
    public long LovId { get; private set; }
    public long LovTypeId { get; private set; }
    public string LovName { get; private set; } = string.Empty;
    public long LovUpdatedBy { get; private set; }
    public DateTime LovUpdatedOn { get; private set; }

    public LovType? LovType { get; private set; }

    private LovMaster() { }

    public static LovMaster Create(long lovId, long lovTypeId, string lovName, long updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lovName);
        if (lovName.Length > 30)
            throw new ArgumentException("LovName cannot exceed 30 characters.", nameof(lovName));

        return new LovMaster
        {
            LovId = lovId,
            LovTypeId = lovTypeId,
            LovName = lovName,
            LovUpdatedBy = updatedBy,
            LovUpdatedOn = DateTime.UtcNow
        };
    }

    public void Update(string lovName, long updatedBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lovName);
        if (lovName.Length > 30)
            throw new ArgumentException("LovName cannot exceed 30 characters.", nameof(lovName));

        LovName = lovName;
        LovUpdatedBy = updatedBy;
        LovUpdatedOn = DateTime.UtcNow;
    }
}
