using NotificationService.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace NotificationService.Infrastructure.Messaging;

public class NotificationCreatedConsumer : IConsumer<NotificationCreatedEvent>
{
    private readonly ILogger<NotificationCreatedConsumer> _logger;
    public NotificationCreatedConsumer(ILogger<NotificationCreatedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<NotificationCreatedEvent> context)
    {
        _logger.LogInformation("Notification created: {Id} - {Title}, Priority: {Priority}", context.Message.NotificationId, context.Message.Title, context.Message.Priority);
        return Task.CompletedTask;
    }
}

public class NotificationReadConsumer : IConsumer<NotificationReadEvent>
{
    private readonly ILogger<NotificationReadConsumer> _logger;
    public NotificationReadConsumer(ILogger<NotificationReadConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<NotificationReadEvent> context)
    {
        _logger.LogInformation("Notification {Id} read by user {UserId}", context.Message.NotificationId, context.Message.UserId);
        return Task.CompletedTask;
    }
}
