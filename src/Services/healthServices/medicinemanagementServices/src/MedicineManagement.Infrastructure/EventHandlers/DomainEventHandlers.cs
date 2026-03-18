using MediatR;
using MedicineManagement.Domain.Events;
using MedicineManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace MedicineManagement.Infrastructure.EventHandlers;

public class MedicineTypeCreatedEventHandler(ILogger<MedicineTypeCreatedEventHandler> logger)
    : INotificationHandler<MedicineTypeCreatedEvent>
{
    public Task Handle(MedicineTypeCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: MedicineType created - Code={Code}, Name={Name}",
            notification.MedicineType.TypeCode, notification.MedicineType.TypeName);
        return Task.CompletedTask;
    }
}

public class MedicineCreatedEventHandler(ILogger<MedicineCreatedEventHandler> logger)
    : INotificationHandler<MedicineCreatedEvent>
{
    public Task Handle(MedicineCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Medicine created - Code={Code}, Name={Name}",
            notification.Medicine.MedicineCode, notification.Medicine.MedicineName);
        return Task.CompletedTask;
    }
}

public class StockTransactionCreatedEventHandler(
    ILogger<StockTransactionCreatedEventHandler> logger,
    IMessagePublisher messagePublisher)
    : INotificationHandler<StockTransactionCreatedEvent>
{
    public async Task Handle(StockTransactionCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Stock transaction created - Medicine={Code}, Type={Type}, Qty={Qty}",
            notification.MedicineCredit.MedicineCode,
            notification.MedicineCredit.RecordType,
            notification.MedicineCredit.Quantity);

        await messagePublisher.PublishAsync("medicine.events", "stock.transaction",
            new { notification.MedicineCredit.MedicineCode, notification.MedicineCredit.RecordType, notification.MedicineCredit.Quantity }, ct);
    }
}

public class MedicineIssuedEventHandler(ILogger<MedicineIssuedEventHandler> logger)
    : INotificationHandler<MedicineIssuedEvent>
{
    public Task Handle(MedicineIssuedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Medicine issued - Code={Code}, Qty={Qty}, Visit={Visit}",
            notification.MedicineIssue.MedicineCode,
            notification.MedicineIssue.IssuedQuantity,
            notification.MedicineIssue.VisitNumber);
        return Task.CompletedTask;
    }
}

public class PurchaseCreatedEventHandler(
    ILogger<PurchaseCreatedEventHandler> logger,
    IMessagePublisher messagePublisher)
    : INotificationHandler<PurchaseCreatedEvent>
{
    public async Task Handle(PurchaseCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Purchase created - Company={Company}, TxnNum={TxnNum}, Vendor={Vendor}",
            notification.Purchase.CompanyCode,
            notification.Purchase.TransactionNumber,
            notification.Purchase.VendorName);

        await messagePublisher.PublishAsync("medicine.events", "purchase.created",
            new { notification.Purchase.CompanyCode, notification.Purchase.TransactionNumber, notification.Purchase.VendorName }, ct);
    }
}

public class LowStockDetectedEventHandler(
    ILogger<LowStockDetectedEventHandler> logger,
    IMessagePublisher messagePublisher)
    : INotificationHandler<LowStockDetectedEvent>
{
    public async Task Handle(LowStockDetectedEvent notification, CancellationToken ct)
    {
        logger.LogWarning("Domain Event: Low stock detected - Medicine={Code} ({Name}), Stock={Stock}, Min={Min}",
            notification.MedicineCode, notification.MedicineName,
            notification.CurrentStock, notification.MinLevel);

        await messagePublisher.PublishAsync("medicine.events", "stock.low",
            new { notification.MedicineCode, notification.MedicineName, notification.CurrentStock, notification.MinLevel }, ct);
    }
}
