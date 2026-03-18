using MediatR;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.Application.Commands.CompleteTask;

public record CompleteTaskCommand : IRequest<WorkTaskDto>
{
    public long TaskId { get; init; }
    public int ActualHours { get; init; }
    public string? CompletionRemarks { get; init; }
    public long CompletedBy { get; init; }
}
