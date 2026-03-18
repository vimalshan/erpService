using ErrorLoggingService.Infrastructure.Messaging.Events;
using MassTransit;

namespace ErrorLoggingService.Infrastructure.Messaging.Publishers;

public sealed class ErrorLogMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ErrorLogMessagePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync(ErrorLoggedMessage message, CancellationToken cancellationToken = default)
        => _publishEndpoint.Publish(message, cancellationToken);
}
