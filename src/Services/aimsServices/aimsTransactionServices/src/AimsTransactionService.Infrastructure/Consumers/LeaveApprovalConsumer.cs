using MassTransit;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Domain.Events;

namespace AimsTransactionService.Infrastructure.Consumers;

public class LeaveApprovalConsumer(ILogger<LeaveApprovalConsumer> logger)
    : IConsumer<LeaveApprovedEvent>
{
    public async Task Consume(ConsumeContext<LeaveApprovedEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation(
            "Processing leave approval for leave {LeaveDetailId}, employee {EmployeeSysId} — Status: {Status}",
            @event.LeaveDetailId, @event.EmployeeSysId, @event.Status);

        // Integration point: update leave credit, notify employee
        await Task.CompletedTask;
    }
}
