namespace ProductionManagement.Application.DTOs;

public record ProductionPlanDto
{
    public int ProductionPlantId { get; init; }
    public int SciItemId { get; init; }
    public int QtyPerDay { get; init; }
    public decimal PlanStartDate { get; init; }
    public DateTime? PlanClosureDate { get; init; }
    public int ModifiedBy { get; init; }
    public DateTime ModifiedDate { get; init; }
}

public record CreateProductionPlanDto(
    int ProductionPlantId,
    int SciItemId,
    int QtyPerDay,
    decimal PlanStartDate,
    int ModifiedBy);

public record UpdateProductionPlanDto(
    int ProductionPlantId,
    int SciItemId,
    int QtyPerDay,
    int ModifiedBy);
