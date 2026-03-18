using MediatR;
using MedicineManagement.Application.Features.Medicines.Queries;
using MedicineManagement.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MedicineManagement.AzureFunctions.Functions;

public class ExpiryCheckFunction(IMediator mediator, IMessagePublisher messagePublisher, ILogger<ExpiryCheckFunction> logger)
{
    /// <summary>
    /// Runs daily at midnight to check for expired medicines and low stock.
    /// </summary>
    [Function("ExpiryAndLowStockCheck")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("ExpiryAndLowStockCheck function started at {Time}", DateTime.UtcNow);

        // Check low stock
        var lowStock = await mediator.Send(new GetLowStockMedicinesQuery(), ct);
        foreach (var item in lowStock)
        {
            logger.LogWarning("Low stock: {Medicine} ({Code}) - Current: {Stock}, Min: {Min}",
                item.MedicineName, item.MedicineCode, item.CurrentStock, item.MinLevel);

            await messagePublisher.PublishAsync("medicine.events", "stock.low", item, ct);
        }

        logger.LogInformation("ExpiryAndLowStockCheck completed. Found {Count} low-stock items.", lowStock.Count);
    }
}
