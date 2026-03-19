namespace ProductionManagement.Application.DTOs;

public record MamProductionDetDto
{
    public int? Id { get; init; }
    public long? ProductionNo { get; init; }
    public DateTime? ProductionDate { get; init; }
    public int? ProductionFg { get; init; }
    public decimal? ProductionQty { get; init; }
}

public record CreateMamProductionDetDto(
    long? ProductionNo,
    DateTime? ProductionDate,
    int? ProductionFg,
    decimal? ProductionQty);

public record MamProductionMapDto
{
    public int? Id { get; init; }
    public int? RmCode { get; init; }
    public int? FgCode { get; init; }
    public decimal? SlNo { get; init; }
}

public record CreateMamProductionMapDto(
    int? RmCode,
    int? FgCode,
    decimal? SlNo);
