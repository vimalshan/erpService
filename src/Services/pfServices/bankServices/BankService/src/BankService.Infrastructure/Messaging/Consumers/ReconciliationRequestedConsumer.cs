using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Messaging.Consumers;

public class ReconciliationRequestedConsumer(IUnitOfWork unitOfWork, ILogger<ReconciliationRequestedConsumer> logger)
    : IConsumer<ReconciliationRequestedMessage>
{
    public async Task Consume(ConsumeContext<ReconciliationRequestedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing ReconciliationRequested message for ChequeId: {ChequeId}", message.ChequeId);

        var recon = PaymentReconciliation.Create(
            message.ChequeId, message.ReconReference,
            message.ReconAmount, DateTime.UtcNow);

        await unitOfWork.PaymentReconciliations.AddAsync(recon);
        await unitOfWork.SaveChangesAsync();
    }
}
