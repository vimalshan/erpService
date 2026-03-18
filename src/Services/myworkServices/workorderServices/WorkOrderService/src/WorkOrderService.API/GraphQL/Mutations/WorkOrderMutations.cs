using MediatR;
using WorkOrderService.Application.Commands.AssignTask;
using WorkOrderService.Application.Commands.CompleteTask;
using WorkOrderService.Application.Commands.CreateWorkOrder;
using WorkOrderService.Application.DTOs;

namespace WorkOrderService.API.GraphQL.Mutations;

public class WorkOrderMutations
{
    public async Task<WorkOrderDto> CreateWorkOrder(
        [Service] IMediator mediator,
        string workOrderName,
        string description,
        DateTime dueDate,
        long assignedTo,
        long createdBy,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateWorkOrderCommand
        {
            WorkOrderName = workOrderName,
            Description = description,
            DueDate = dueDate,
            AssignedTo = assignedTo,
            CreatedBy = createdBy
        }, cancellationToken);
    }

    public async Task<WorkTaskDto> AssignTask(
        [Service] IMediator mediator,
        long workOrderId,
        string taskName,
        long assignedTo,
        int estimatedHours,
        long createdBy,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new AssignTaskCommand
        {
            WorkOrderId = workOrderId,
            TaskName = taskName,
            AssignedTo = assignedTo,
            EstimatedHours = estimatedHours,
            CreatedBy = createdBy
        }, cancellationToken);
    }

    public async Task<WorkTaskDto> CompleteTask(
        [Service] IMediator mediator,
        long taskId,
        int actualHours,
        string? completionRemarks,
        long completedBy,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CompleteTaskCommand
        {
            TaskId = taskId,
            ActualHours = actualHours,
            CompletionRemarks = completionRemarks,
            CompletedBy = completedBy
        }, cancellationToken);
    }
}
