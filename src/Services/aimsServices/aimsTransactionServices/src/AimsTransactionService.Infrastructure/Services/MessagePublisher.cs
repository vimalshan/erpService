using MassTransit;
using AimsTransactionService.Application.Common.Interfaces;

namespace AimsTransactionService.Infrastructure.Services;

public class MessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public async Task PublishAsync<T>(
        T message,
        string routingKey,
        CancellationToken cancellationToken = default) where T : class
    {
        await publishEndpoint.Publish(message, cancellationToken);
    }
}
