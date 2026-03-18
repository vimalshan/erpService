using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.ValueObjects;

namespace WorkOrderService.Domain.Entities;

public class WorkTask : BaseEntity
{
    public long TaskId { get; private set; }
    public long WorkOrderId { get; private set; }
    public string TaskName { get; private set; } = string.Empty;
    public long AssignedTo { get; private set; }
    public int EstimatedHours { get; private set; }
    public int? ActualHours { get; private set; }
    public WorkTaskStatus TaskStatus { get; private set; } = WorkTaskStatus.Open;
    public string? CompletionRemarks { get; private set; }
    public long? CompletedBy { get; private set; }
    public DateTime? CompletedOn { get; private set; }

    // Navigation
    public WorkOrder WorkOrder { get; private set; } = null!;

    private WorkTask() { }

    public WorkTask(long workOrderId, string taskName, long assignedTo, int estimatedHours, long createdBy)
    {
        WorkOrderId = workOrderId;
        TaskName = taskName;
        AssignedTo = assignedTo;
        EstimatedHours = estimatedHours;
        TaskStatus = WorkTaskStatus.Open;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public void Complete(int actualHours, string? completionRemarks, long completedBy)
    {
        TaskStatus = WorkTaskStatus.Completed;
        ActualHours = actualHours;
        CompletionRemarks = completionRemarks;
        CompletedBy = completedBy;
        CompletedOn = DateTime.UtcNow;
        UpdatedBy = completedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Pause(long updatedBy)
    {
        TaskStatus = WorkTaskStatus.Paused;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Reopen(long updatedBy)
    {
        TaskStatus = WorkTaskStatus.Open;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Archive(long updatedBy)
    {
        TaskStatus = WorkTaskStatus.Archived;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
