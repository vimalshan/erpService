namespace ProductionManagement.Application.DTOs;

public record ProductionPlanEntryDto
{
    public int? Id { get; init; }
    public string? OracleCode { get; init; }
    public string? Month { get; init; }
    public char? ProType { get; init; }
    public int? ProValue { get; init; }
    public int? FactoryId { get; init; }
    public string? Zone { get; init; }
    public int? ProYear { get; init; }
}

public record CreateProductionPlanEntryDto(
    string? OracleCode,
    string? Month,
    char? ProType,
    int? ProValue,
    int? FactoryId,
    string? Zone,
    int? ProYear);
