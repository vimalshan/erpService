using Microsoft.Extensions.Logging;

namespace InsuranceManagement.Infrastructure.MessageConsumers;

/// <summary>
/// Event message for enrollment events
/// </summary>
public class EnrollmentEventMessage
{
    public Guid EventId { get; set; }
    public long EnrollmentId { get; set; }
    public long EmpSysId { get; set; }
    public long PlanId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Details { get; set; } = string.Empty;
}

/// <summary>
/// Event message for claim events
/// </summary>
public class ClaimEventMessage
{
    public Guid EventId { get; set; }
    public long ClaimId { get; set; }
    public long EnrollmentId { get; set; }
    public long EmpSysId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public decimal ClaimAmount { get; set; }
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Details { get; set; } = string.Empty;
}

/// <summary>
/// Consumer for enrollment events from RabbitMQ
/// </summary>
public class EnrollmentEventConsumer : RabbitMqConsumer<EnrollmentEventMessage>
{
    private readonly ILogger<EnrollmentEventConsumer> _logger;

    public EnrollmentEventConsumer(
        IRabbitMqConnectionFactory connectionFactory,
        ILogger<EnrollmentEventConsumer> logger)
        : base(
            connectionFactory,
            logger,
            queueName: "insurance.enrollment.events",
            exchangeName: "insurance.events",
            routingKey: "enrollment.*")
    {
        _logger = logger;
    }

    protected override async Task ProcessMessageAsync(EnrollmentEventMessage message)
    {
        try
        {
            LogMessage($"Processing enrollment event: {message.EventType} for enrollment {message.EnrollmentId}");

            // Process different event types
            switch (message.EventType)
            {
                case "EnrollmentCreated":
                    await HandleEnrollmentCreatedAsync(message);
                    break;

                case "EnrollmentTerminated":
                    await HandleEnrollmentTerminatedAsync(message);
                    break;

                case "EnrollmentSuspended":
                    await HandleEnrollmentSuspendedAsync(message);
                    break;

                case "EnrollmentReactivated":
                    await HandleEnrollmentReactivatedAsync(message);
                    break;

                default:
                    LogMessage($"Unknown enrollment event type: {message.EventType}", LogLevel.Warning);
                    break;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            LogMessage($"Error processing enrollment event: {ex.Message}", LogLevel.Error);
            throw;
        }
    }

    private async Task HandleEnrollmentCreatedAsync(EnrollmentEventMessage message)
    {
        LogMessage($"Enrollment created for employee {message.EmpSysId} in plan {message.PlanId}");
        // TODO: Send notification email, update employee records, etc.
        await Task.CompletedTask;
    }

    private async Task HandleEnrollmentTerminatedAsync(EnrollmentEventMessage message)
    {
        LogMessage($"Enrollment {message.EnrollmentId} terminated");
        // TODO: Update employee records, send termination notice, etc.
        await Task.CompletedTask;
    }

    private async Task HandleEnrollmentSuspendedAsync(EnrollmentEventMessage message)
    {
        LogMessage($"Enrollment {message.EnrollmentId} suspended");
        // TODO: Pause benefits, send notification, etc.
        await Task.CompletedTask;
    }

    private async Task HandleEnrollmentReactivatedAsync(EnrollmentEventMessage message)
    {
        LogMessage($"Enrollment {message.EnrollmentId} reactivated");
        // TODO: Resume benefits, send notification, etc.
        await Task.CompletedTask;
    }
}

/// <summary>
/// Consumer for claim events from RabbitMQ
/// </summary>
public class ClaimEventConsumer : RabbitMqConsumer<ClaimEventMessage>
{
    private readonly ILogger<ClaimEventConsumer> _logger;

    public ClaimEventConsumer(
        IRabbitMqConnectionFactory connectionFactory,
        ILogger<ClaimEventConsumer> logger)
        : base(
            connectionFactory,
            logger,
            queueName: "insurance.claim.events",
            exchangeName: "insurance.events",
            routingKey: "claim.*")
    {
        _logger = logger;
    }

    protected override async Task ProcessMessageAsync(ClaimEventMessage message)
    {
        try
        {
            LogMessage($"Processing claim event: {message.EventType} for claim {message.ClaimId}");

            switch (message.EventType)
            {
                case "ClaimSubmitted":
                    await HandleClaimSubmittedAsync(message);
                    break;

                case "ClaimApproved":
                    await HandleClaimApprovedAsync(message);
                    break;

                case "ClaimRejected":
                    await HandleClaimRejectedAsync(message);
                    break;

                case "ClaimPaid":
                    await HandleClaimPaidAsync(message);
                    break;

                default:
                    LogMessage($"Unknown claim event type: {message.EventType}", LogLevel.Warning);
                    break;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            LogMessage($"Error processing claim event: {ex.Message}", LogLevel.Error);
            throw;
        }
    }

    private async Task HandleClaimSubmittedAsync(ClaimEventMessage message)
    {
        LogMessage($"Claim {message.ClaimId} submitted for amount {message.ClaimAmount}");
        // TODO: Create audit entry, notify insurance team, etc.
        await Task.CompletedTask;
    }

    private async Task HandleClaimApprovedAsync(ClaimEventMessage message)
    {
        LogMessage($"Claim {message.ClaimId} approved");
        // TODO: Trigger payment processing, send notification to employee, etc.
        await Task.CompletedTask;
    }

    private async Task HandleClaimRejectedAsync(ClaimEventMessage message)
    {
        LogMessage($"Claim {message.ClaimId} rejected");
        // TODO: Send rejection notice to employee with reason, create audit entry, etc.
        await Task.CompletedTask;
    }

    private async Task HandleClaimPaidAsync(ClaimEventMessage message)
    {
        LogMessage($"Claim {message.ClaimId} marked as paid");
        // TODO: Update financial records, send payment confirmation, etc.
        await Task.CompletedTask;
    }
}
