using FillingOperationService.Domain.Common;

namespace FillingOperationService.Domain.Entities;

public class FillingPointGroup : Entity
{
    public int FillingPointGroupId { get; private set; }
    public string FillingPointGroupName { get; private set; } = string.Empty;
    public int FillingLineId { get; private set; }
    public int NoOfFillingPoints { get; private set; }
    public int? ExclusiveUse { get; private set; }
    public string? IsClosed { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    protected FillingPointGroup() { }

    public static FillingPointGroup Create(int lineId, string name, int noOfPoints, int? exclusiveUse, int createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Filling point group name is required.");
        return new FillingPointGroup
        {
            FillingLineId = lineId,
            FillingPointGroupName = name.Trim(),
            NoOfFillingPoints = noOfPoints,
            ExclusiveUse = exclusiveUse,
            IsClosed = "N",
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
    }
}
