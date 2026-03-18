using MassTransit;
using Microsoft.Extensions.Logging;

namespace TimesheetService.Infrastructure.Messaging.Consumers;

/// <summary>
/// Receives approval request messages from other services via RabbitMQ.
/// </summary>
public sealed class TimesheetApprovalRequestConsumer : IConsumer<TimesheetApprovalRequestMessage>
{
    private readonly ILogger<TimesheetApprovalRequestConsumer> _logger;

    public TimesheetApprovalRequestConsumer(ILogger<TimesheetApprovalRequestConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<TimesheetApprovalRequestMessage> context)
    {
        _logger.LogInformation(
            "Received approval request for Timesheet {TimesheetId} from Employee {EmployeeId}",
            context.Message.TimesheetId, context.Message.EmployeeId);

        // TODO: Dispatch ApproveTimesheetCommand or business logic here
        return Task.CompletedTask;
    }
}

public sealed class TimesheetApprovalRequestMessage
{
    public long TimesheetId { get; set; }
    public long EmployeeId  { get; set; }
    public long ApproverId  { get; set; }
}
