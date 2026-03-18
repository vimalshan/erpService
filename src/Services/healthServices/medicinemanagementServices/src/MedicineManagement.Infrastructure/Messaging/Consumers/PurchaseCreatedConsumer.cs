using MedicineManagement.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedicineManagement.Infrastructure.Messaging.Consumers;

public class PurchaseCreatedConsumer(
    string hostName, string userName, string password,
    IServiceProvider serviceProvider,
    ILogger<PurchaseCreatedConsumer> logger)
    : RabbitMqConsumerBase<PurchaseMainDto>(
        hostName, userName, password,
        "medicine.purchase.created", "medicine.events", "purchase.created",
        logger)
{
    protected override async Task HandleMessageAsync(PurchaseMainDto message, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        Logger.LogInformation("Purchase created event received: Company={Company}, TxnNum={TxnNum}",
            message.CompanyCode, message.TransactionNumber);
        await Task.CompletedTask;
    }
}
