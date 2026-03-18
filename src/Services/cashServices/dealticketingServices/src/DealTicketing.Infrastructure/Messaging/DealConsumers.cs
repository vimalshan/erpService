using DealTicketing.Application.DTOs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Infrastructure.Messaging;

// ── Message contracts ──────────────────────────────────────────────────────

public record DealBatchCreatedMessage(long BatchId, DateTime DealDate, long DerType, DateTime OccurredAt);
public record DealApprovedMessage(long DealId, long BatchId, long AppBusiness, DateTime OccurredAt);
public record DealRejectedMessage(long DealId, long BatchId, string Remarks, DateTime OccurredAt);
public record DealSettledMessage(long DealId, long SettlementId, decimal GainLossAmt, DateTime OccurredAt);

// ── Consumers ─────────────────────────────────────────────────────────────

public class DealBatchCreatedConsumer(ILogger<DealBatchCreatedConsumer> logger)
    : IConsumer<DealBatchCreatedMessage>
{
    public Task Consume(ConsumeContext<DealBatchCreatedMessage> context)
    {
        logger.LogInformation(
            "Deal batch created: BatchId={BatchId}, Date={Date}, DerType={DerType}",
            context.Message.BatchId, context.Message.DealDate, context.Message.DerType);
        // Downstream: notify approvers, trigger workflow, etc.
        return Task.CompletedTask;
    }
}

public class DealApprovedConsumer(ILogger<DealApprovedConsumer> logger)
    : IConsumer<DealApprovedMessage>
{
    public Task Consume(ConsumeContext<DealApprovedMessage> context)
    {
        logger.LogInformation(
            "Deal approved: DealId={DealId}, Business={Business}",
            context.Message.DealId, context.Message.AppBusiness);
        // Downstream: send confirmation to bank, update ledger, etc.
        return Task.CompletedTask;
    }
}

public class DealRejectedConsumer(ILogger<DealRejectedConsumer> logger)
    : IConsumer<DealRejectedMessage>
{
    public Task Consume(ConsumeContext<DealRejectedMessage> context)
    {
        logger.LogWarning(
            "Deal rejected: DealId={DealId}, Remarks={Remarks}",
            context.Message.DealId, context.Message.Remarks);
        // Downstream: notify deal owner, log audit trail
        return Task.CompletedTask;
    }
}

public class DealSettledConsumer(ILogger<DealSettledConsumer> logger)
    : IConsumer<DealSettledMessage>
{
    public Task Consume(ConsumeContext<DealSettledMessage> context)
    {
        logger.LogInformation(
            "Deal settled: DealId={DealId}, GainLoss={GainLoss}",
            context.Message.DealId, context.Message.GainLossAmt);
        // Downstream: post to P&L, trigger accounting entries
        return Task.CompletedTask;
    }
}
