namespace ExitManagement.Application.DTOs;

public record EmployeeExitDto(
    decimal ExitNo,
    decimal EmployeeSysId,
    DateTime? LetterGivenOn,
    DateTime? ExpectedRelieveDate,
    string? ResignationType,
    decimal ResignationId,
    string? Remarks,
    string? Status,
    DateTime? RelieveGivenOn,
    DateTime? InterviewConductedOn,
    string? InterviewConductedBy,
    string? RevokeReason,
    DateTime? RevokeDate,
    string? ApprovalStatus,
    decimal? ApprovedBy,
    DateTime? ApprovedOn,
    string? PayrollSettlement,
    DateTime? StopSalaryDate,
    string? DesignationOnJoining,
    string? ReasonForLeaving
);

public record ExitInterviewFeedbackDto(
    decimal ExitNo,
    decimal SerialNo,
    string? QuestionId,
    string? Feedback,
    decimal? UpdatedBy,
    DateTime? UpdatedOn
);

public record ExitQuestionDto(
    string? QuestionId,
    string? QuestionDescription,
    decimal? QuestionOrder
);

public record ExitInterviewQuestionDto(
    string? QuestionId,
    string? QuestionDescription,
    decimal? OrderId
);

public record ExitResponsibilityMapDto(
    decimal? TtId,
    decimal? EmployeeSysId,
    decimal? ChecklistMapId,
    string? Primary,
    string? Secondary,
    string? FunctionalHead
);
