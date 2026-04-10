namespace ProblemManagement.Application.DTOs;

public record ProblemDto
{
    public long PrId { get; init; }
    public long PrOwner { get; init; }
    public long PrEnteredBy { get; init; }
    public string PrDescription { get; init; } = string.Empty;
    public DateTime? PrRespExpBy { get; init; }
    public string? PrCategory { get; init; }
    public long? PrSpecialization { get; init; }
    public string? PrImpact { get; init; }
    public string? PrExpResult { get; init; }
    public DateTime? PrEnteredOn { get; init; }
    public string PrStatus { get; init; }
    public string? PrStatement { get; init; }
    public string? PrType { get; init; }
    public long PrUnitId { get; init; }
    public long PrSiteId { get; init; }
    public DateTime PrModOn { get; init; }
    public List<ProblemSolutionDto> Solutions { get; init; } = [];
    public List<ProblemAttachmentDto> Attachments { get; init; } = [];
    public List<ProblemApprovalDto> Approvals { get; init; } = [];
}

public record ProblemSolutionDto
{
    public long SolId { get; init; }
    public long SolPrId { get; init; }
    public string? SolDescription { get; init; }
    public string? SolImplementation { get; init; }
    public long SolEnteredBy { get; init; }
    public DateTime SolEnteredOn { get; init; }
    public string? SolAttach { get; init; }
    public List<SolutionCommentDto> Comments { get; init; } = [];
    public List<SolutionApprovalDto> Approvals { get; init; } = [];
}

public record ProblemApprovalDto
{
    public long PrAppId { get; init; }
    public long PrAppPrId { get; init; }
    public long PrAppBy { get; init; }
    public DateTime PrAppOn { get; init; }
    public string PrAppStatus { get; init; }
    public string? PrAppReason { get; init; }
    public string PrAppAudFlag { get; init; }
}

public record ProblemAttachmentDto
{
    public long PratId { get; init; }
    public long? PratPrId { get; init; }
    public string? PratFileName { get; init; }
    public DateTime? PratEnteredOn { get; init; }
}

public record SolutionApprovalDto
{
    public long SolAppId { get; init; }
    public long SolAppSolId { get; init; }
    public long SolAppBy { get; init; }
    public DateTime SolAppOn { get; init; }
    public string SolAppStatus { get; init; }
    public string? SolAppReason { get; init; }
}

public record SolutionCommentDto
{
    public long SolCommentId { get; init; }
    public long SolCommentSolId { get; init; }
    public string SolCommentText { get; init; } = string.Empty;
    public long SolCommentBy { get; init; }
    public DateTime SolCommentOn { get; init; }
}

public record ProblemFunctionDto
{
    public long FuncId { get; init; }
    public string FuncName { get; init; } = string.Empty;
}

public record ProblemImpactDto
{
    public long ImpactId { get; init; }
    public string ImpactDesc { get; init; } = string.Empty;
}
