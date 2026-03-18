using BankService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Messaging.Consumers;

public class ChequeClearedConsumer(IUnitOfWork unitOfWork, ILogger<ChequeClearedConsumer> logger)
    : IConsumer<ChequeClearedMessage>
{
    public async Task Consume(ConsumeContext<ChequeClearedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing ChequeCleared message for ChequeId: {ChequeId}", message.ChequeId);

        var cheque = await unitOfWork.ChequeDetails.GetByIdAsync(message.ChequeId);
        if (cheque is not null)
        {
            cheque.Clear(message.ClearedDate);
            unitOfWork.ChequeDetails.Update(cheque);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
