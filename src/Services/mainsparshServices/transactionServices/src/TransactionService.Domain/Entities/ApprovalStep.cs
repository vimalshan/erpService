using TransactionService.Domain.Common;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Entities;

public sealed class ApprovalStep : BaseEntity
{
    public long WorkflowId { get; private set; }
    public int StepLevel { get; private set; }
    public long ApproverId { get; private set; }
    public string StepStatus { get; private set; } = "PENDING";
    public string? StepRemarks { get; private set; }
    public DateTime? ActedOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }

    public ApprovalWorkflow? Workflow { get; private set; }

    private ApprovalStep() { }

    public static ApprovalStep Create(long workflowId, int stepLevel, long approverId, long createdBy)
    {
        if (stepLevel < 1) throw new DomainException("Step level must be at least 1.");
        if (approverId <= 0) throw new DomainException("ApproverId must be positive.");

        return new ApprovalStep
        {
            WorkflowId = workflowId,
            StepLevel = stepLevel,
            ApproverId = approverId,
            StepStatus = "PENDING",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void Approve(long approverId, string? remarks)
    {
        if (StepStatus != "PENDING")
            throw new DomainException($"Step is not pending. Current status: {StepStatus}");

        StepStatus = "APPROVED";
        StepRemarks = remarks;
        ActedOn = DateTime.UtcNow;
    }

    public void Reject(long approverId, string? remarks)
    {
        if (StepStatus != "PENDING")
            throw new DomainException($"Step is not pending. Current status: {StepStatus}");

        StepStatus = "REJECTED";
        StepRemarks = remarks;
        ActedOn = DateTime.UtcNow;
    }

    public void Skip()
    {
        if (StepStatus == "PENDING")
        {
            StepStatus = "SKIPPED";
            ActedOn = DateTime.UtcNow;
        }
    }
}
