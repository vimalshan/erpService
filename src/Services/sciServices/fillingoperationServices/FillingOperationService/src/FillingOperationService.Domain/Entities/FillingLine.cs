using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Events;

namespace FillingOperationService.Domain.Entities;

public class FillingLine : Entity
{
    public int FillingLineId { get; private set; }
    public int FillingPlantId { get; private set; }
    public string FillingLineName { get; private set; } = string.Empty;
    public int NoOfFillingPoints { get; private set; }
    public int? PackageTypeId { get; private set; }
    public string? IsClosed { get; private set; }
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private readonly List<FillingPointGroup> _pointGroups = new();
    public IReadOnlyCollection<FillingPointGroup> FillingPointGroups => _pointGroups.AsReadOnly();

    protected FillingLine() { }

    public static FillingLine Create(int plantId, string name, int noOfFillingPoints, int? packageTypeId, int createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Filling line name is required.");
        if (noOfFillingPoints <= 0)
            throw new ArgumentException("Number of filling points must be positive.");

        var line = new FillingLine
        {
            FillingPlantId = plantId,
            FillingLineName = name.Trim(),
            NoOfFillingPoints = noOfFillingPoints,
            PackageTypeId = packageTypeId,
            IsClosed = "N",
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };
        line.AddDomainEvent(new FillingLineCreatedEvent(line));
        return line;
    }

    public void Close(int modifiedBy)
    {
        IsClosed = "Y";
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }
}
