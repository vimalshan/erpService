using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkOrderService.Application.Commands.CompleteTask;

namespace WorkOrderService.Infrastructure.Messaging.Consumers;

public record TaskCompletionMessage
{
    public long TaskId { get; init; }
    public int ActualHours { get; init; }
    public string? CompletionRemarks { get; init; }
    public long CompletedBy { get; init; }
}

public class TaskCompletionConsumer : BaseMessageConsumer<TaskCompletionMessage>
{
    public TaskCompletionConsumer(IServiceProvider serviceProvider, ILogger<TaskCompletionConsumer> logger)
        : base(serviceProvider, logger, "workorder.task.completion")
    {
    }

    protected override async Task HandleMessageAsync(TaskCompletionMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CompleteTaskCommand
        {
            TaskId = message.TaskId,
            ActualHours = message.ActualHours,
            CompletionRemarks = message.CompletionRemarks,
            CompletedBy = message.CompletedBy
        }, cancellationToken);
    }
}
