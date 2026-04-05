namespace TransactionService.Application.DTOs;

public record ApprovalWorkflowDto(
    long WorkflowId,
    string WorkflowCode,
    string EntityType,
    long EntityId,
    long EmployeeId,
    string WorkflowStatus,
    int CurrentApprovalLevel,
    long? CurrentApproverId,
    int MaxApprovalLevels,
    string? Remarks,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn,
    List<ApprovalStepDto> Steps
);

public record ApprovalStepDto(
    long StepId,
    long WorkflowId,
    int StepLevel,
    long ApproverId,
    string StepStatus,
    string? StepRemarks,
    DateTime? ActedOn,
    long CreatedBy,
    DateTime CreatedOn
);

public record TransactionLogDto(
    long LogId,
    string TransactionType,
    long TransactionId,
    string Action,
    long ActionBy,
    string? ActionData,
    string? PreviousStatus,
    string? NewStatus,
    string? IpAddress,
    DateTime CreatedOn
);

public record PendingApprovalDto(
    long WorkflowId,
    string WorkflowCode,
    string EntityType,
    long EntityId,
    long EmployeeId,
    string WorkflowStatus,
    int CurrentApprovalLevel,
    long? CurrentApproverId,
    int MaxApprovalLevels
);

public record StoredProcResultDto(
    bool Success,
    string? Message,
    IEnumerable<dynamic>? Data
);
