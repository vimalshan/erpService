namespace ProductionManagement.Application.DTOs;

public record ProductionPlantProductMapDto
{
    public int ProductionPlantId { get; init; }
    public int ProductId { get; init; }
    public int SciUserIdCreated { get; init; }
    public DateTime CreationDate { get; init; }
}

public record CreateProductionPlantProductMapDto(
    int ProductionPlantId,
    int ProductId,
    int CreatedBy);
