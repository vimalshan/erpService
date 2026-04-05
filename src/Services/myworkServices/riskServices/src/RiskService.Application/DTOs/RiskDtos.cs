namespace RiskService.Application.DTOs;

public record RiskDto
{
    public long Id { get; init; }
    public string ApplicableTo { get; init; } = default!;
    public long OrganizationId { get; init; }
    public long BusinessId { get; init; }
    public long DivisionId { get; init; }
    public long UnitId { get; init; }
    public long FunctionId { get; init; }
    public string EventTitle { get; init; } = default!;
    public string Description { get; init; } = default!;
    public long TypeId { get; init; }
    public string? TypeName { get; init; }
    public long ImpactId { get; init; }
    public long ProbabilityId { get; init; }
    public long RatingId { get; init; }
    public long ResidualImpactId { get; init; }
    public long ResidualProbabilityId { get; init; }
    public long ResidualRatingId { get; init; }
    public long ResponseId { get; init; }
    public string MitigationFlag { get; init; } = default!;
    public long OwnerId { get; init; }
    public string ApprovalStatus { get; init; } = default!;
    public DateTime? CancelDate { get; init; }
    public string? CancelReason { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public long? ModifiedBy { get; init; }
    public DateTime? ModifiedOn { get; init; }
    public List<RiskCauseDto> Causes { get; init; } = new();
    public List<RiskControlDto> Controls { get; init; } = new();
    public List<RiskImpactMapDto> ImpactMaps { get; init; } = new();
    public List<RiskEventDto> Events { get; init; } = new();
    public List<MitigationDto> Mitigations { get; init; } = new();
}

public record RiskCauseDto(long Id, long RiskId, string Description);
public record RiskControlDto(long Id, long RiskId, string Description, string FileName, long? ImpactReductionPercent, long? ProbabilityReductionPercent);
public record RiskImpactMapDto(long Id, long RiskId, string Description);
public record RiskEventDto(long Id, long RiskId, string Description, DateTime EventDate);

public record MitigationDto
{
    public long Id { get; init; }
    public long RiskId { get; init; }
    public string Action { get; init; } = default!;
    public DateTime OriginalDueDate { get; init; }
    public DateTime DueDate { get; init; }
    public long OwnerId { get; init; }
    public long ReviewerId { get; init; }
    public string Status { get; init; } = default!;
    public decimal? ProbabilityReduction { get; init; }
    public decimal? ImpactReduction { get; init; }
    public string? Attachment { get; init; }
    public List<MitigationActionDto> Actions { get; init; } = new();
}

public record MitigationActionDto(long Id, long MitigationId, DateTime DueDate, string Status, string ApprovalStatus, string Comments, DateTime? CompletionDate);

public record SelfAssessmentDto
{
    public long Id { get; init; }
    public string AssessmentType { get; init; } = default!;
    public long TypeReferenceId { get; init; }
    public string MonitoredBy { get; init; } = default!;
    public DateTime DueDate { get; init; }
    public string MeetingFlag { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string? Reason { get; init; }
    public DateTime AssessmentDate { get; init; }
    public string ApprovalStatus { get; init; } = default!;
}

public record RiskTypeDto(long Id, string Name);
public record RiskImpactDto(long Id, long Rank, string Name);
public record RiskProbabilityDto(long Id, long Rank, string Name, string Occurrence);
public record RiskRatingDto(long Id, long Rank, long RatingFrom, long RatingTo, string Name);
public record RiskResponseDto(long Id, string Name);
public record RiskDivisionDto(long Id, string Name, long HrmsBusinessId);
public record RiskFunctionDto(long Id, string Name);
public record RiskUnitChampionDto(long Id, long EmployeeSysId, string ChampionType, long OrganizationId, long BusinessId, long DivisionId, long UnitId);
