namespace ProductionManagement.Application.DTOs;

public record ProductionPlantDto
{
    public int ProductionPlantId { get; init; }
    public int CompanyUnitId { get; init; }
    public string PlantName { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public int? CreatedBy { get; init; }
    public DateTime? CreationDate { get; init; }
    public int? ModifiedBy { get; init; }
    public DateTime? ModifiedDate { get; init; }
}

public record CreateProductionPlantDto(
    int CompanyUnitId,
    string PlantName,
    string Location,
    int CreatedBy);

public record UpdateProductionPlantDto(
    int ProductionPlantId,
    string PlantName,
    string Location,
    int ModifiedBy);
