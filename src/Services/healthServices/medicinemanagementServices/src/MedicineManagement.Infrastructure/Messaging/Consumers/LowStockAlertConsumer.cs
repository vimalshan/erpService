using MedicineManagement.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedicineManagement.Infrastructure.Messaging.Consumers;

public class LowStockAlertConsumer(
    string hostName, string userName, string password,
    IServiceProvider serviceProvider,
    ILogger<LowStockAlertConsumer> logger)
    : RabbitMqConsumerBase<StockSummaryDto>(
        hostName, userName, password,
        "medicine.stock.low", "medicine.events", "stock.low",
        logger)
{
    protected override async Task HandleMessageAsync(StockSummaryDto message, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        Logger.LogWarning("Low stock alert: Medicine={MedicineCode}, Current={Stock}, Min={Min}",
            message.MedicineCode, message.CurrentStock, message.MinLevel);
        await Task.CompletedTask;
    }
}
