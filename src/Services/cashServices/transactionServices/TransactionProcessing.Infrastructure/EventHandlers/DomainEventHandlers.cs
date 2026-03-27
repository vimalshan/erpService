using MediatR;
using Microsoft.Extensions.Logging;
using TransactionProcessing.Domain.Events;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Infrastructure.EventHandlers;

public sealed class TransactionRecordedEventHandler(
    ILogger<TransactionRecordedEventHandler> logger,
    IBlobStorageService blobStorage) : INotificationHandler<TransactionRecordedEvent>
{
    public async Task Handle(TransactionRecordedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Transaction {TxnId} recorded: {TxnType} for {Amount} from {Source}",
            notification.TxnId, notification.TxnType, notification.Amount, notification.SourceService);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
            System.Text.Json.JsonSerializer.Serialize(notification)));
        await blobStorage.UploadAsync("transaction-events",
            $"recorded/{notification.TxnId}_{DateTime.UtcNow:yyyyMMddHHmmss}.json", stream, ct);
    }
}

public sealed class SettlementProcessedEventHandler(
    ILogger<SettlementProcessedEventHandler> logger) : INotificationHandler<SettlementProcessedEvent>
{
    public Task Handle(SettlementProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Settlement {SettlementId} processed for Deal {DealId}, Net: {NetAmount}",
            notification.SettlementId, notification.DealId, notification.NetAmount);
        return Task.CompletedTask;
    }
}

public sealed class DisbursementProcessedEventHandler(
    ILogger<DisbursementProcessedEventHandler> logger) : INotificationHandler<DisbursementProcessedEvent>
{
    public Task Handle(DisbursementProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Disbursement {DisbProcId} processed for Loan {LoanId}, Amount: {Amount}",
            notification.DisbProcId, notification.LoanId, notification.Amount);
        return Task.CompletedTask;
    }
}

public sealed class RepaymentProcessedEventHandler(
    ILogger<RepaymentProcessedEventHandler> logger) : INotificationHandler<RepaymentProcessedEvent>
{
    public Task Handle(RepaymentProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Repayment {RepayProcId} processed for Loan {LoanId}, Amount: {Amount}",
            notification.RepayProcId, notification.LoanId, notification.Amount);
        return Task.CompletedTask;
    }
}

public sealed class BatchCompletedEventHandler(
    ILogger<BatchCompletedEventHandler> logger,
    IBlobStorageService blobStorage) : INotificationHandler<BatchCompletedEvent>
{
    public async Task Handle(BatchCompletedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Batch {BatchId} completed: {SuccessCount} success, {FailureCount} failures, Total: {TotalAmount}",
            notification.BatchId, notification.SuccessCount, notification.FailureCount, notification.TotalAmount);

        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(
            System.Text.Json.JsonSerializer.Serialize(notification)));
        await blobStorage.UploadAsync("transaction-events",
            $"batch-completed/{notification.BatchId}_{DateTime.UtcNow:yyyyMMddHHmmss}.json", stream, ct);
    }
}
