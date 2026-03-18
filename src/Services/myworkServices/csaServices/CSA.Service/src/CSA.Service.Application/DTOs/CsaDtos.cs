namespace CSA.Service.Application.DTOs;

public record ControlDto(
    long ControlId,
    string Title,
    string? Description,
    char? ControlType,
    char? ControlMethod,
    string? Risk,
    char? Priority,
    long? ProcessId,
    long? SubProcessId,
    char? Periodicity,
    char? EvidenceFlag,
    char? ApproverFlag,
    long? CreatedBy,
    DateTime? CreatedOn
);

public record CreateControlDto(
    string Title,
    string? Description,
    char? ControlType,
    char? ControlMethod,
    string? Risk,
    char? Priority,
    long? ProcessId,
    long? SubProcessId,
    char? Periodicity,
    char? EvidenceFlag,
    char? ApproverFlag
);

public record UpdateControlDto(
    long ControlId,
    string Title,
    string? Description,
    char? ControlType,
    char? ControlMethod,
    string? Risk,
    char? Priority,
    long? ProcessId,
    long? SubProcessId,
    char? Periodicity,
    char? EvidenceFlag,
    char? ApproverFlag
);

public record EvidenceDto(
    long EvidenceId,
    long ControlId,
    string? Name,
    string? TempName
);

public record SurveyDto(
    long SurveyId,
    string Title,
    DateTime DueDate,
    DateTime CloseDate,
    DateTime StartDate,
    DateTime EndDate,
    long? Alert1,
    long? Alert2,
    long? CreatedBy,
    DateTime? CreatedOn
);

public record CreateSurveyDto(
    string Title,
    DateTime DueDate,
    DateTime CloseDate,
    DateTime StartDate,
    DateTime EndDate,
    long? Alert1,
    long? Alert2
);

public record SurveyQuestionDto(
    long SurveyQuestionId,
    long SurveyId,
    long ControlId,
    long UnitId,
    long OwnerId,
    long ApproverId,
    DateTime OriginalDueDate,
    DateTime DueDate,
    char? AssessmentFlag,
    char? ApprovalFlag,
    char? RemedialFlag
);

public record SurveyFeedbackDto(
    long FeedbackId,
    long SurveyQuestionId,
    long EmployeeSysId,
    char Status,
    char Type,
    char RemedialFlag,
    string? Remarks,
    DateTime EnteredOn,
    char EvidenceFlag,
    char ApprovalFlag,
    string ApproverRemarks
);

public record ProcessDto(
    long ProcessId,
    string Name,
    long? CreatedBy,
    DateTime? CreatedOn
);

public record CreateProcessDto(string Name);

public record SubProcessDto(
    long SubProcessId,
    long ProcessId,
    string Name,
    long? CreatedBy,
    DateTime? CreatedOn
);

public record CreateSubProcessDto(long ProcessId, string Name);

public record UnitDto(
    long UnitId,
    string Name,
    string ShortName,
    string Code,
    long BusinessId,
    char LiveFlag,
    long OrgId
);

public record UnitMapDetailDto(
    long MapId,
    long ControlId,
    long UnitId,
    long OwnerId,
    long ApproverId,
    char ReportingManager,
    DateTime EffectiveDate,
    DateTime? ClosureDate,
    DateTime DueDate
);
