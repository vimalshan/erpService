using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.Events;
using WorkOrderService.Domain.ValueObjects;

namespace WorkOrderService.Domain.Entities;

public class WorkOrder : AggregateRoot
{
    private readonly List<WorkTask> _tasks = [];

    public long WorkOrderId { get; private set; }
    public string WorkOrderName { get; private set; } = string.Empty;
    public string WorkOrderDescription { get; private set; } = string.Empty;
    public DateTime DueDate { get; private set; }
    public long AssignedTo { get; private set; }
    public WorkOrderStatus WorkOrderStatus { get; private set; } = WorkOrderStatus.Open;

    public IReadOnlyCollection<WorkTask> Tasks => _tasks.AsReadOnly();

    private WorkOrder() { }

    public WorkOrder(string name, string description, DateTime dueDate, long assignedTo, long createdBy)
    {
        WorkOrderName = name;
        WorkOrderDescription = description;
        DueDate = dueDate;
        AssignedTo = assignedTo;
        WorkOrderStatus = WorkOrderStatus.Open;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;

        AddDomainEvent(new WorkOrderCreatedEvent(this));
    }

    public WorkTask AddTask(string taskName, long assignedTo, int estimatedHours, long createdBy)
    {
        var task = new WorkTask(WorkOrderId, taskName, assignedTo, estimatedHours, createdBy);
        _tasks.Add(task);

        AddDomainEvent(new TaskAssignedEvent(WorkOrderId, task));
        return task;
    }

    public void CompleteTask(long taskId, int actualHours, string? completionRemarks, long completedBy)
    {
        var task = _tasks.FirstOrDefault(t => t.TaskId == taskId)
            ?? throw new InvalidOperationException($"Task {taskId} not found in work order {WorkOrderId}");

        task.Complete(actualHours, completionRemarks, completedBy);
        AddDomainEvent(new TaskCompletedEvent(WorkOrderId, task));

        // Auto-close work order if all tasks are completed
        if (_tasks.Count > 0 && _tasks.All(t => t.TaskStatus == WorkTaskStatus.Completed))
        {
            Close(completedBy);
        }
    }

    public void Close(long updatedBy)
    {
        WorkOrderStatus = WorkOrderStatus.Closed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WorkOrderStatusChangedEvent(WorkOrderId, WorkOrderStatus.Closed));
    }

    public void Archive(long updatedBy)
    {
        WorkOrderStatus = WorkOrderStatus.Archived;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WorkOrderStatusChangedEvent(WorkOrderId, WorkOrderStatus.Archived));
    }

    public int GetCompletionPercentage()
    {
        if (_tasks.Count == 0) return 0;
        var completed = _tasks.Count(t => t.TaskStatus == WorkTaskStatus.Completed);
        return (int)(completed * 100.0 / _tasks.Count);
    }
}
