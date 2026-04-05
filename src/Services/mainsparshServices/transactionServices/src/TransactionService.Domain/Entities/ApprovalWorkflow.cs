using TransactionService.Domain.Common;
using TransactionService.Domain.Events;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Entities;

public sealed class ApprovalWorkflow : AuditableEntity
{
    public string WorkflowCode { get; private set; } = null!;
    public string EntityType { get; private set; } = null!;
    public long EntityId { get; private set; }
    public long EmployeeId { get; private set; }
    public string WorkflowStatus { get; private set; } = "SUBMITTED";
    public int CurrentApprovalLevel { get; private set; } = 1;
    public long? CurrentApproverId { get; private set; }
    public int MaxApprovalLevels { get; private set; } = 1;
    public string? Remarks { get; private set; }

    private readonly List<ApprovalStep> _steps = new();
    public IReadOnlyCollection<ApprovalStep> Steps => _steps.AsReadOnly();

    private ApprovalWorkflow() { }

    public static ApprovalWorkflow Create(
        string entityType,
        long entityId,
        long employeeId,
        long currentApproverId,
        int maxApprovalLevels,
        string? remarks,
        long createdBy)
    {
        if (string.IsNullOrWhiteSpace(entityType)) throw new DomainException("EntityType is required.");
        if (entityId <= 0) throw new DomainException("EntityId must be positive.");
        if (employeeId <= 0) throw new DomainException("EmployeeId must be positive.");
        if (maxApprovalLevels < 1) throw new DomainException("MaxApprovalLevels must be at least 1.");

        var workflow = new ApprovalWorkflow
        {
            WorkflowCode = $"WF-{entityType}-{DateTime.UtcNow:yyyyMMddHHmmss}-{entityId}",
            EntityType = entityType,
            EntityId = entityId,
            EmployeeId = employeeId,
            WorkflowStatus = "SUBMITTED",
            CurrentApprovalLevel = 1,
            CurrentApproverId = currentApproverId,
            MaxApprovalLevels = maxApprovalLevels,
            Remarks = remarks,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        var firstStep = ApprovalStep.Create(workflow.Id, 1, currentApproverId, createdBy);
        workflow._steps.Add(firstStep);

        workflow.AddDomainEvent(new WorkflowSubmittedEvent(workflow.Id, workflow.EntityType, workflow.EntityId, workflow.EmployeeId));
        return workflow;
    }

    public void ApproveCurrentStep(long approverId, string? remarks)
    {
        if (WorkflowStatus is "APPROVED" or "REJECTED" or "CANCELLED")
            throw new DomainException($"Cannot approve a workflow in {WorkflowStatus} status.");

        var currentStep = _steps.FirstOrDefault(s => s.StepLevel == CurrentApprovalLevel && s.StepStatus == "PENDING")
            ?? throw new DomainException($"No pending step found at level {CurrentApprovalLevel}.");

        currentStep.Approve(approverId, remarks);

        if (CurrentApprovalLevel >= MaxApprovalLevels)
        {
            WorkflowStatus = "APPROVED";
            AddDomainEvent(new WorkflowApprovedEvent(Id, EntityType, EntityId, CurrentApprovalLevel, approverId));
        }
        else
        {
            CurrentApprovalLevel++;
            WorkflowStatus = "IN_REVIEW";
        }

        Remarks = remarks;
        UpdatedBy = approverId;
        UpdatedOn = DateTime.UtcNow;
    }

    public void RejectCurrentStep(long approverId, string? remarks)
    {
        if (WorkflowStatus is "APPROVED" or "REJECTED" or "CANCELLED")
            throw new DomainException($"Cannot reject a workflow in {WorkflowStatus} status.");

        var currentStep = _steps.FirstOrDefault(s => s.StepLevel == CurrentApprovalLevel && s.StepStatus == "PENDING")
            ?? throw new DomainException($"No pending step found at level {CurrentApprovalLevel}.");

        currentStep.Reject(approverId, remarks);
        WorkflowStatus = "REJECTED";
        Remarks = remarks;
        UpdatedBy = approverId;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WorkflowRejectedEvent(Id, EntityType, EntityId, CurrentApprovalLevel, approverId, remarks));
    }

    public void Cancel(long cancelledBy, string? remarks)
    {
        if (WorkflowStatus is "APPROVED" or "REJECTED" or "CANCELLED")
            throw new DomainException($"Cannot cancel a workflow in {WorkflowStatus} status.");

        WorkflowStatus = "CANCELLED";
        Remarks = remarks;
        UpdatedBy = cancelledBy;
        UpdatedOn = DateTime.UtcNow;

        foreach (var step in _steps.Where(s => s.StepStatus == "PENDING"))
            step.Skip();

        AddDomainEvent(new WorkflowCancelledEvent(Id, EntityType, EntityId, cancelledBy));
    }

    public void AddNextStep(long approverId, long createdBy)
    {
        var nextLevel = _steps.Count + 1;
        if (nextLevel > MaxApprovalLevels)
            throw new DomainException("Cannot add steps beyond max approval levels.");

        var step = ApprovalStep.Create(Id, nextLevel, approverId, createdBy);
        _steps.Add(step);
        CurrentApproverId = approverId;
    }
}
