using MediatR;
using Microsoft.Extensions.Logging;
using CashManagement.Domain.Events;
using CashManagement.Domain.Interfaces;

namespace CashManagement.Infrastructure.EventHandlers;

public class CashReceiptRecordedEventHandler : INotificationHandler<CashReceiptRecordedEvent>
{
    private readonly ILogger<CashReceiptRecordedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public CashReceiptRecordedEventHandler(ILogger<CashReceiptRecordedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(CashReceiptRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CashReceipt recorded — Unit {UnitId}, Amount {Amount}, Ref {Ref}",
            notification.CashUnitId, notification.Amount, notification.RefNo);
        await _publisher.PublishAsync("cash.receipt.recorded", notification);
    }
}

public class CashDisbursementRecordedEventHandler : INotificationHandler<CashDisbursementRecordedEvent>
{
    private readonly ILogger<CashDisbursementRecordedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public CashDisbursementRecordedEventHandler(ILogger<CashDisbursementRecordedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(CashDisbursementRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CashDisbursement recorded — Unit {UnitId}, Amount {Amount}, Ref {Ref}",
            notification.CashUnitId, notification.Amount, notification.RefNo);
        await _publisher.PublishAsync("cash.disbursement.recorded", notification);
    }
}

public class ChequeIssuedEventHandler : INotificationHandler<ChequeIssuedEvent>
{
    private readonly ILogger<ChequeIssuedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public ChequeIssuedEventHandler(ILogger<ChequeIssuedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(ChequeIssuedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Cheque issued — Account {AccountId}, Cheque# {Number}, Payee {Payee}, Amount {Amount}",
            notification.BankAccountId, notification.ChequeNumber, notification.PayeeName, notification.Amount);
        await _publisher.PublishAsync("cheque.issued", notification);
    }
}

public class ChequeBouncedEventHandler : INotificationHandler<ChequeBouncedEvent>
{
    private readonly ILogger<ChequeBouncedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public ChequeBouncedEventHandler(ILogger<ChequeBouncedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(ChequeBouncedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Domain Event: Cheque BOUNCED — Account {AccountId}, Cheque# {Number}, Reason: {Reason}, Amount {Amount}",
            notification.BankAccountId, notification.ChequeNumber, notification.Reason, notification.Amount);
        await _publisher.PublishAsync("cheque.bounced", notification);
    }
}

public class BankTransactionRecordedEventHandler : INotificationHandler<BankTransactionRecordedEvent>
{
    private readonly ILogger<BankTransactionRecordedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public BankTransactionRecordedEventHandler(ILogger<BankTransactionRecordedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(BankTransactionRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: BankTransaction recorded — Account {AccountId}, Type {Type}, Amount {Amount}",
            notification.BankAccountId, notification.TxnType, notification.Amount);
        await _publisher.PublishAsync("bank.transaction.recorded", notification);
    }
}

public class CashUnitCreatedEventHandler : INotificationHandler<CashUnitCreatedEvent>
{
    private readonly ILogger<CashUnitCreatedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public CashUnitCreatedEventHandler(ILogger<CashUnitCreatedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(CashUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: CashUnit created — ID {Id}, Name {Name}",
            notification.CashUnitId, notification.Name);
        await _publisher.PublishAsync("cashunit.created", notification);
    }
}

public class BankAccountCreatedEventHandler : INotificationHandler<BankAccountCreatedEvent>
{
    private readonly ILogger<BankAccountCreatedEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public BankAccountCreatedEventHandler(ILogger<BankAccountCreatedEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(BankAccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: BankAccount created — ID {Id}, Bank {Name}, Account {AccNo}",
            notification.BankAccountId, notification.BankName, notification.AccountNo);
        await _publisher.PublishAsync("bankaccount.created", notification);
    }
}

public class BankReconciledEventHandler : INotificationHandler<BankReconciledEvent>
{
    private readonly ILogger<BankReconciledEventHandler> _logger;
    private readonly IEventPublisher _publisher;
    public BankReconciledEventHandler(ILogger<BankReconciledEventHandler> logger, IEventPublisher publisher)
    { _logger = logger; _publisher = publisher; }

    public async Task Handle(BankReconciledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Bank reconciled — Account {AccountId}, Date {Date}, Diff {Diff}",
            notification.BankAccountId, notification.ReconciliationDate, notification.Difference);
        await _publisher.PublishAsync("bank.reconciled", notification);
    }
}
