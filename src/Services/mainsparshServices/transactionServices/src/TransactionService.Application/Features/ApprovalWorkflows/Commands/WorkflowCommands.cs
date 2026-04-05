using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Features.ApprovalWorkflows.Commands;

public record SubmitWorkflowCommand(
    string EntityType,
    long EntityId,
    long EmployeeId,
    long CurrentApproverId,
    int MaxApprovalLevels,
    string? Remarks,
    long CreatedBy
) : IRequest<ApprovalWorkflowDto>;

public record ApproveStepCommand(
    long WorkflowId,
    long ApproverId,
    string? Remarks,
    long? NextApproverId
) : IRequest<ApprovalWorkflowDto>;

public record RejectStepCommand(
    long WorkflowId,
    long ApproverId,
    string? Remarks
) : IRequest<ApprovalWorkflowDto>;

public record CancelWorkflowCommand(
    long WorkflowId,
    long CancelledBy,
    string? Remarks
) : IRequest<bool>;
