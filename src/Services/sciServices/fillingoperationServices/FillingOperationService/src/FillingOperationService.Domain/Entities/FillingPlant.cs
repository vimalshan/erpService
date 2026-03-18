using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Events;
using FillingOperationService.Domain.ValueObjects;

namespace FillingOperationService.Domain.Entities;

public class FillingPlant : AggregateRoot
{
    public int FillingPlantId { get; private set; }
    public int CompanyUnitId { get; private set; }
    public string FillingPlantName { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public int SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private readonly List<FillingLine> _fillingLines = new();
    public IReadOnlyCollection<FillingLine> FillingLines => _fillingLines.AsReadOnly();

    protected FillingPlant() { }

    public static FillingPlant Create(int companyUnitId, string plantName, string location, int createdBy)
    {
        var plant = new FillingPlant
        {
            CompanyUnitId = companyUnitId,
            FillingPlantName = PlantName.Create(plantName).Value,
            Location = ValueObjects.Location.Create(location).Value,
            SciUserIdCreated = createdBy,
            CreationDate = DateTime.UtcNow
        };

        plant.AddDomainEvent(new FillingPlantRegisteredEvent(plant));
        return plant;
    }

    public void Update(string plantName, string location, int modifiedBy)
    {
        FillingPlantName = PlantName.Create(plantName).Value;
        Location = ValueObjects.Location.Create(location).Value;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void AddFillingLine(FillingLine line) => _fillingLines.Add(line);
}
