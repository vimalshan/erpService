using TransactionService.Domain.Common;
using TransactionService.Domain.ValueObjects;

namespace TransactionService.Domain.Entities;

public sealed class TransactionLog : BaseEntity
{
    public string TransactionType { get; private set; } = string.Empty;
    public long TransactionId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public long ActionBy { get; private set; }
    public string? ActionData { get; private set; }
    public string? PreviousStatus { get; private set; }
    public string? NewStatus { get; private set; }
    public string? IpAddress { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private TransactionLog() { }

    public static TransactionLog Create(
        string transactionType,
        long transactionId,
        string action,
        long actionBy,
        string? actionData = null,
        string? previousStatus = null,
        string? newStatus = null,
        string? ipAddress = null)
    {
        var log = new TransactionLog
        {
            TransactionType = transactionType,
            TransactionId = transactionId,
            Action = action,
            ActionBy = actionBy,
            ActionData = actionData,
            PreviousStatus = previousStatus,
            NewStatus = newStatus,
            IpAddress = ipAddress,
            CreatedOn = DateTime.UtcNow
        };

        log.AddDomainEvent(new Events.TransactionLoggedEvent(log.Id, transactionType, transactionId, action));

        return log;
    }
}
