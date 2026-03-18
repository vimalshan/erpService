using MassTransit;
using Microsoft.Extensions.Logging;
using ReimbursementService.Infrastructure.Messaging.Contracts;

namespace ReimbursementService.Infrastructure.Messaging.Consumers;

/// <summary>Handles ReimbursementSubmittedMessage — e.g., notify approvers.</summary>
public sealed class ReimbursementSubmittedConsumer(ILogger<ReimbursementSubmittedConsumer> logger)
    : IConsumer<ReimbursementSubmittedMessage>
{
    public Task Consume(ConsumeContext<ReimbursementSubmittedMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation(
            "[RabbitMQ] Reimbursement submitted: RefNo={RefNo}, EmpId={EmpSysId}, Amount={Amount} {Currency}, OccurredOn={OccurredOn}",
            msg.RefNo, msg.EmpSysId, msg.Amount, msg.Currency, msg.OccurredOn);
        // TODO: Send notification / trigger approval workflow
        return Task.CompletedTask;
    }
}

/// <summary>Handles ReimbursementApprovedMessage — e.g., trigger payment processing.</summary>
public sealed class ReimbursementApprovedConsumer(ILogger<ReimbursementApprovedConsumer> logger)
    : IConsumer<ReimbursementApprovedMessage>
{
    public Task Consume(ConsumeContext<ReimbursementApprovedMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation(
            "[RabbitMQ] Reimbursement approved: ReimId={ReimId}, ApprovedBy={ApprovedBy}, Level={ApprovalLevel}",
            msg.ReimId, msg.ApprovedBy, msg.ApprovalLevel);
        // TODO: Initiate payment order
        return Task.CompletedTask;
    }
}

/// <summary>Handles ReimbursementPaidMessage — e.g., update accounting system.</summary>
public sealed class ReimbursementPaidConsumer(ILogger<ReimbursementPaidConsumer> logger)
    : IConsumer<ReimbursementPaidMessage>
{
    public Task Consume(ConsumeContext<ReimbursementPaidMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation(
            "[RabbitMQ] Reimbursement paid: ReimId={ReimId}, PaymentDate={PaymentDate}",
            msg.ReimId, msg.PaymentDate);
        // TODO: Post journal entry to accounting
        return Task.CompletedTask;
    }
}

