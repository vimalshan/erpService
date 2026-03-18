using BankService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankService.Application.EventHandlers;

public class BankCreatedEventHandler(ILogger<BankCreatedEventHandler> logger)
    : INotificationHandler<BankCreatedEvent>
{
    public Task Handle(BankCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Bank created - TrustCode: {TrustCode}, BankCode: {BankCode}",
            notification.TrustCode, notification.BankCode);
        return Task.CompletedTask;
    }
}

public class BankAccountCreatedEventHandler(ILogger<BankAccountCreatedEventHandler> logger)
    : INotificationHandler<BankAccountCreatedEvent>
{
    public Task Handle(BankAccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Bank Account created - Number: {AccountNumber}, Title: {Title}",
            notification.AccountNumber, notification.AccountTitle);
        return Task.CompletedTask;
    }
}

public class ChequeIssuedEventHandler(ILogger<ChequeIssuedEventHandler> logger)
    : INotificationHandler<ChequeIssuedEvent>
{
    public Task Handle(ChequeIssuedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Cheque Issued - ID: {ChequeId}, Amount: {Amount}, Payee: {Payee}",
            notification.ChequeId, notification.Amount, notification.Payee);
        return Task.CompletedTask;
    }
}

public class ChequeClearedEventHandler(ILogger<ChequeClearedEventHandler> logger)
    : INotificationHandler<ChequeClearedEvent>
{
    public Task Handle(ChequeClearedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Cheque Cleared - ID: {ChequeId}, Date: {ClearedDate}",
            notification.ChequeId, notification.ClearedDate);
        return Task.CompletedTask;
    }
}

public class ReconciliationCompletedEventHandler(ILogger<ReconciliationCompletedEventHandler> logger)
    : INotificationHandler<ReconciliationCompletedEvent>
{
    public Task Handle(ReconciliationCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Reconciliation Completed - ReconId: {ReconId}, ChequeId: {ChequeId}",
            notification.ReconId, notification.ChequeId);
        return Task.CompletedTask;
    }
}
