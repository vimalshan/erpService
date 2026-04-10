using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.Aggregates;

/// <summary>Maps to DOC_CORRESPONDATT</summary>
public class CorrespondenceAttachment : Entity<long>
{
    public long CorrespondenceId { get; private set; }
    public string CorrespondenceStatus { get; private set; } = default!;  // H/R
    public string FilePath { get; private set; } = default!;

    private CorrespondenceAttachment() { }

    public static CorrespondenceAttachment Create(long id, long correspondenceId, string status, string filePath)
    {
        return new CorrespondenceAttachment
        {
            Id = id,
            CorrespondenceId = correspondenceId,
            CorrespondenceStatus = status,
            FilePath = filePath
        };
    }
}

/// <summary>Maps to DOC_DEFECTIVEATT</summary>
public class DefectiveAttachment : Entity<long>
{
    public long AllocationId { get; private set; }
    public string FilePath { get; private set; } = default!;

    private DefectiveAttachment() { }

    public static DefectiveAttachment Create(long id, long allocationId, string filePath)
    {
        return new DefectiveAttachment
        {
            Id = id,
            AllocationId = allocationId,
            FilePath = filePath
        };
    }
}
