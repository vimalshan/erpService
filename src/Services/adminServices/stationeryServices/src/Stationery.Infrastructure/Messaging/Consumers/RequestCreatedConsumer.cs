using MassTransit;
using Microsoft.Extensions.Logging;
using Stationery.Domain.Events;

namespace Stationery.Infrastructure.Messaging.Consumers;

public class RequestCreatedConsumer : IConsumer<RequestCreatedEvent>
{
    private readonly ILogger<RequestCreatedConsumer> _logger;

    public RequestCreatedConsumer(ILogger<RequestCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RequestCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Asynchronously processing new request: {RequestId} from User: {UserId}", 
            message.Request.Id, message.Request.RequestedBy);
        
        // Simulating some background work like sending an email or notifying a warehouse
        await Task.Delay(100); 
        
        _logger.LogInformation("Background processing completed for Request: {RequestId}", message.Request.Id);
    }
}
