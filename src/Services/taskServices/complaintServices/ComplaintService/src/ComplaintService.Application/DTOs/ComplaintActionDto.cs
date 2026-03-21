namespace ComplaintService.Application.DTOs;

public record ComplaintActionDto(
    decimal ActionNum,
    decimal TaskNum,
    string? PrimaryResp,
    decimal? PrimaryActBy,
    DateTime? PrimaryActDate,
    string? PrimarySolution,
    string? SecResp,
    decimal? SecActBy,
    DateTime? SecActDate,
    string? SecSolution,
    string? FwdResp,
    decimal? FwdActBy,
    DateTime? FwdActDate,
    string? FwdSolution,
    decimal? CurrentEscLevel,
    string? CorrResp,
    decimal? CorrActBy,
    DateTime? CorrActDate,
    string? CorrSolution,
    bool IsReopened,
    string? ReopenRemarks,
    DateTime? TargetDate,
    DateTime? ClosureDate
);
