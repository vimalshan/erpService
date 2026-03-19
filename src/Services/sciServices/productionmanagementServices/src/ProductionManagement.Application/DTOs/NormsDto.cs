namespace ProductionManagement.Application.DTOs;

public record NormsMainDto
{
    public long NormNo { get; init; }
    public DateTime NormEffDate { get; init; }
    public DateTime? NormClsDate { get; init; }
    public List<NormsMasterDto>? NormsMasters { get; init; }
}

public record CreateNormsMainDto(
    long NormNo,
    DateTime NormEffDate);

public record NormsMasterDto
{
    public long? NormId { get; init; }
    public int? NormInputCode { get; init; }
    public int? NormOutputCode { get; init; }
    public int? NormRate { get; init; }
    public long? NormNo { get; init; }
}

public record CreateNormsMasterDto(
    long NormId,
    int NormInputCode,
    int NormOutputCode,
    int NormRate,
    long NormNo);
