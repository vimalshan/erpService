namespace WorkOrderService.Application.DTOs;

public record WorkOrderDto
{
    public long WorkOrderId { get; init; }
    public string WorkOrderName { get; init; } = string.Empty;
    public string WorkOrderDescription { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public long AssignedTo { get; init; }
    public string WorkOrderStatus { get; init; } = string.Empty;
    public string WorkOrderStatusCode { get; init; } = string.Empty;
    public int CompletionPercentage { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public long? UpdatedBy { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public List<WorkTaskDto> Tasks { get; init; } = [];
}
