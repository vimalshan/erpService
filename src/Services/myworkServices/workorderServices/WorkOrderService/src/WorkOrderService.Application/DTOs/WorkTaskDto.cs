namespace WorkOrderService.Application.DTOs;

public record WorkTaskDto
{
    public long TaskId { get; init; }
    public long WorkOrderId { get; init; }
    public string TaskName { get; init; } = string.Empty;
    public long AssignedTo { get; init; }
    public int EstimatedHours { get; init; }
    public int? ActualHours { get; init; }
    public string TaskStatus { get; init; } = string.Empty;
    public string TaskStatusCode { get; init; } = string.Empty;
    public string? CompletionRemarks { get; init; }
    public long? CompletedBy { get; init; }
    public DateTime? CompletedOn { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public long? UpdatedBy { get; init; }
    public DateTime? UpdatedOn { get; init; }
}
