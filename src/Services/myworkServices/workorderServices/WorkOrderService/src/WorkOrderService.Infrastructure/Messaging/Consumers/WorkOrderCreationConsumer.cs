using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkOrderService.Application.Commands.CreateWorkOrder;

namespace WorkOrderService.Infrastructure.Messaging.Consumers;

public record WorkOrderCreationMessage
{
    public string WorkOrderName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public long AssignedTo { get; init; }
    public long CreatedBy { get; init; }
}

public class WorkOrderCreationConsumer : BaseMessageConsumer<WorkOrderCreationMessage>
{
    public WorkOrderCreationConsumer(IServiceProvider serviceProvider, ILogger<WorkOrderCreationConsumer> logger)
        : base(serviceProvider, logger, "workorder.creation")
    {
    }

    protected override async Task HandleMessageAsync(WorkOrderCreationMessage message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new CreateWorkOrderCommand
        {
            WorkOrderName = message.WorkOrderName,
            Description = message.Description,
            DueDate = message.DueDate,
            AssignedTo = message.AssignedTo,
            CreatedBy = message.CreatedBy
        }, cancellationToken);
    }
}
