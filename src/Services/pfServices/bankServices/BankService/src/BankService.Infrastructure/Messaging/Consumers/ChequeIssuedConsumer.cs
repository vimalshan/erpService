using BankService.Domain.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Messaging.Consumers;

public class ChequeIssuedConsumer(IUnitOfWork unitOfWork, ILogger<ChequeIssuedConsumer> logger)
    : IConsumer<ChequeIssuedMessage>
{
    public async Task Consume(ConsumeContext<ChequeIssuedMessage> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing ChequeIssued message for ChequeId: {ChequeId}", message.ChequeId);

        var cheque = await unitOfWork.ChequeDetails.GetByIdAsync(message.ChequeId);
        if (cheque is not null)
        {
            cheque.MarkOutstanding();
            unitOfWork.ChequeDetails.Update(cheque);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
