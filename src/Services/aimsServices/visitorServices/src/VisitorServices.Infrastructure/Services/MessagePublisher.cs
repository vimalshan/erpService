using MassTransit;
using VisitorServices.Application.Common.Interfaces;

namespace VisitorServices.Infrastructure.Services;

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
