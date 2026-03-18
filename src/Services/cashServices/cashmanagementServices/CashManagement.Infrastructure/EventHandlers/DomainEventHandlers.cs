using MediatR;
using Microsoft.Extensions.Logging;
using CashManagement.Domain.Events;

namespace CashManagement.Infrastructure.EventHandlers;

public class CashReceiptRecordedEventHandler : INotificationHandler<CashReceiptRecordedEvent>
{
    private readonly ILogger<CashReceiptRecordedEventHandler> _logger;
    public CashReceiptRecordedEventHandler(ILogger<CashReceiptRecordedEventHandler> logger) => _logger = logger;

    public Task Handle(CashReceiptRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CashReceipt recorded — Unit {UnitId}, Amount {Amount}, Ref {Ref}",
            notification.CashUnitId, notification.Amount, notification.RefNo);
        return Task.CompletedTask;
    }
}

public class CashDisbursementRecordedEventHandler : INotificationHandler<CashDisbursementRecordedEvent>
{
    private readonly ILogger<CashDisbursementRecordedEventHandler> _logger;
    public CashDisbursementRecordedEventHandler(ILogger<CashDisbursementRecordedEventHandler> logger) => _logger = logger;

    public Task Handle(CashDisbursementRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CashDisbursement recorded — Unit {UnitId}, Amount {Amount}, Ref {Ref}",
            notification.CashUnitId, notification.Amount, notification.RefNo);
        return Task.CompletedTask;
    }
}

public class ChequeIssuedEventHandler : INotificationHandler<ChequeIssuedEvent>
{
    private readonly ILogger<ChequeIssuedEventHandler> _logger;
    public ChequeIssuedEventHandler(ILogger<ChequeIssuedEventHandler> logger) => _logger = logger;

    public Task Handle(ChequeIssuedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Cheque issued — Account {AccountId}, Cheque# {Number}, Payee {Payee}, Amount {Amount}",
            notification.BankAccountId, notification.ChequeNumber, notification.PayeeName, notification.Amount);
        return Task.CompletedTask;
    }
}

public class ChequeBouncedEventHandler : INotificationHandler<ChequeBouncedEvent>
{
    private readonly ILogger<ChequeBouncedEventHandler> _logger;
    public ChequeBouncedEventHandler(ILogger<ChequeBouncedEventHandler> logger) => _logger = logger;

    public Task Handle(ChequeBouncedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Domain Event: Cheque BOUNCED — Account {AccountId}, Cheque# {Number}, Reason: {Reason}, Amount {Amount}",
            notification.BankAccountId, notification.ChequeNumber, notification.Reason, notification.Amount);
        return Task.CompletedTask;
    }
}

public class BankTransactionRecordedEventHandler : INotificationHandler<BankTransactionRecordedEvent>
{
    private readonly ILogger<BankTransactionRecordedEventHandler> _logger;
    public BankTransactionRecordedEventHandler(ILogger<BankTransactionRecordedEventHandler> logger) => _logger = logger;

    public Task Handle(BankTransactionRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: BankTransaction recorded — Account {AccountId}, Type {Type}, Amount {Amount}",
            notification.BankAccountId, notification.TxnType, notification.Amount);
        return Task.CompletedTask;
    }
}
