using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TransactionService.Infrastructure.Messaging.Consumers;

public sealed record BatchApprovedMessage(string BatchId, string ApprovalType, long ApprovedBy, DateTime ApprovedOn);

public sealed class BatchApprovedConsumer : BaseMessageConsumer<BatchApprovedMessage>
{
    protected override string QueueName => "transaction.batch.approved";
    protected override string ExchangeName => "transaction.events";
    protected override string RoutingKey => "transaction.batch.approved";

    public BatchApprovedConsumer(
        IConfiguration configuration, ILogger<BatchApprovedConsumer> logger) : base(configuration, logger) { }

    protected override async Task HandleMessageAsync(
        BatchApprovedMessage message, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "Batch Approved: BatchId={BatchId}, Type={ApprovalType}, ApprovedBy={ApprovedBy}",
            message.BatchId, message.ApprovalType, message.ApprovedBy);
        await Task.CompletedTask;
    }
}
